using System.Runtime.InteropServices.JavaScript;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace DPBack.Infrastructure.Seeder;

public static class UserSeeder
{
    public static async Task SeedAsync(UserStoreDbContext context, IPasswordHasher<User> passwordHasher)
    {
        if (!context.Users.Any(x => x.Login == "admin"))
        {
            var admin = new User(Guid.NewGuid(), "admin", "", "admin@local", UserRole.Admin, DateTime.UtcNow);
            var password = passwordHasher.HashPassword(admin, "1111");
            var entity = new UserEntity(admin.Id, admin.Login, password, admin.Email, admin.Role, admin.CreatedAt);
            context.Users.Add(entity);
            await context.SaveChangesAsync();
        }
    }
}