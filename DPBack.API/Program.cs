using System.Text;
using System.Text.Json.Serialization;
using DPBack.API.Middleware;
using DPBack.API.PayU;
using DPBack.Application.Abstractions;
using DPBack.Application.Commands;
using DPBack.Application.Options;
using DPBack.Application.Options.Pricing;
using DPBack.Application.Pricing;
using DPBack.Application.Pricing.Calculators;
using DPBack.Application.Services;
using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Payments;
using DPBack.Infrastructure.Repositories;
using DPBack.Infrastructure.Seeder;
using DPBack.Infrastructure.TockenProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable cross-api requests
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(["http://localhost:5173","http://localhost:3000" ]) // 5173:AdminPanel, 3000:PublicApp
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateActor = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
        ValidAudience = jwtOptions.Audience,
        ValidIssuer = jwtOptions.Issuer,
    };
});
builder.Services.AddAuthorization();
builder.Services.AddDbContext<OrderStoreDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(OrderStoreDbContext)));
});

builder.Services.AddDbContext<UserStoreDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(UserStoreDbContext)));
});
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IPriceCalcService, PriceCalcService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<PaymentCommands>();

//Pricing

builder.Services.Configure<BusinesscardPricing>(
    builder.Configuration.GetSection("Pricing:Businesscard"));
builder.Services.AddScoped<IPriceCalculator, BusinesscardCalculator>();
builder.Services.Configure<OpeningHoursStickerPricing>(
    builder.Configuration.GetSection("Pricing:WindowStickers:OpeningHours"));
builder.Services.AddScoped<IPriceCalculator, OpeningHoursStickerCalculator>();
builder.Services.AddScoped<PriceCalculatorFactory>();

builder.Services.AddSingleton<IPaymentTokenProvider, PayUTokenProvider>();
builder.Services.AddHttpClient<IPaymentService, PayUService>(client =>
{
    client.BaseAddress = new Uri("https://secure.snd.payu.com");
});

builder.Services.AddSingleton<ITokenProvider,TokenProvider>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var userDb = scope.ServiceProvider.GetRequiredService<UserStoreDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
    await UserSeeder.SeedAsync(userDb, passwordHasher, configuration);
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();