using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Seeder;
using Microsoft.AspNetCore.Identity;

namespace DPBack.API.Extensions;

public static class SeederExtensions
{
    public static async Task SeedDBAsync(this WebApplication app, IConfiguration configuration)
    {
        using (var scope = app.Services.CreateScope())
        {
            var userDb = scope.ServiceProvider.GetRequiredService<UserStoreDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
            await UserSeeder.SeedAsync(userDb, passwordHasher, configuration);
        }
    }
}