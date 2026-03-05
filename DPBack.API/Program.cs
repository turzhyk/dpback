using System.Text;
using DPBack.API.TockenProvider;
using DPBack.Application.Interfaces;
using DPBack.Application.Services;
using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Repositories;
using DPBack.Infrastructure.Seeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(["http://localhost:5173","http://localhost:3000" ]) // адрес твоего React dev
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateActor = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5ab418f6-5d62-4ae7-8afe-a38c73c72a1e"))
    };
});
builder.Services.AddAuthorization();
builder.Services.AddDbContext<OrderStoreDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(OrderStoreDbContext)));
});
builder.Services.AddDbContext<ProductStoreDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ProductStoreDbContext)));
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

builder.Services.AddSingleton<TokenProvider>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var userDb = scope.ServiceProvider.GetRequiredService<UserStoreDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<PasswordHasher<User>>();
    await UserSeeder.SeedAsync(userDb, passwordHasher);
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
// app.MapGet("/", () => "Hello World!");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();