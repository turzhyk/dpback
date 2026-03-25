using DPBack.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DPBack.API.Extensions;

public static class DbContextExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderStoreDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(nameof(OrderStoreDbContext)));
        });

        services.AddDbContext<UserStoreDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(nameof(UserStoreDbContext)));
        });
        return services;
    }
}