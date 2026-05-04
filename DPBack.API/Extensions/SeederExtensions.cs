using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DPBack.API.Extensions;

public static class SeederExtensions
{
    public static async Task SeedDBAsync(this WebApplication app, IConfiguration configuration)
    {
        using (var scope = app.Services.CreateScope())
        {
            var orderDb = scope.ServiceProvider.GetRequiredService<OrderStoreDbContext>();
            await orderDb.Database.MigrateAsync();
            var userDb = scope.ServiceProvider.GetRequiredService<UserStoreDbContext>();
            await userDb.Database.MigrateAsync();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
            await UserSeeder.SeedAsync(userDb, passwordHasher, configuration);
        }
    }
}