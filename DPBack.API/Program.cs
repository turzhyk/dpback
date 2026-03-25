using System.Text.Json.Serialization;
using DPBack.API.Extensions;
using DPBack.API.Middleware;
using DPBack.Application.Abstractions;
using DPBack.Infrastructure.Payments;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
builder.Services.AddOptions(configuration);
builder.Services.AddCorsPolicy(configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDatabase(configuration);
builder.Services.AddApplicationServices(configuration);
builder.Services.AddAuthorizationServices(configuration);

builder.Services.AddHttpClient<IPaymentService, PayUService>(client =>
{
    client.BaseAddress = new Uri("https://secure.snd.payu.com");
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
var app = builder.Build();
await app.SeedDBAsync(configuration);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});
app.UseCors();
app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();