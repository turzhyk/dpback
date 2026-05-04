namespace DPBack.API.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var urls = configuration.GetSection("Cors:Origins").Get < string[] > ();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins(urls) // 5173:AdminPanel, 3000:PublicApp
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        return services;
    }
}