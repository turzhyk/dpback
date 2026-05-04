using DPBack.Application.Abstractions;
using DPBack.Application.Options.Pricing;
using DPBack.Application.Pricing;
using DPBack.Application.Pricing.Calculators;
using DPBack.Application.Services;
using DPBack.Domain.Models;
using DPBack.Infrastructure.PayU;
using DPBack.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DPBack.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IOrdersService, OrdersService>();
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IPriceCalcService, PriceCalcService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.Configure<BusinesscardPricing>(
            configuration.GetSection("Pricing:Businesscard"));
        services.AddScoped<IPriceCalculator, BusinesscardCalculator>();
        services.Configure<OpeningHoursStickerPricing>(
            configuration.GetSection("Pricing:WindowStickers:OpeningHours"));
        services.AddScoped<IPriceCalculator, OpeningHoursStickerCalculator>();
        services.Configure<PhotoPricing>(
            configuration.GetSection("Pricing:Photo"));
        services.AddScoped<IPriceCalculator, PhotoCalculator>();


        services.AddScoped<PriceCalculatorFactory>();

        services.AddSingleton<IPaymentTokenProvider, PayUTokenProvider>();
        return services;
    }
}