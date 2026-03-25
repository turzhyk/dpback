using DPBack.Application.Options;

namespace DPBack.API.Extensions;

public static class OptionsExtensions
{
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<PayUOptions>(configuration.GetSection("PayU"));
        return services;
    }
}