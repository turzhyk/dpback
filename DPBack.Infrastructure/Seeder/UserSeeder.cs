using System.Runtime.InteropServices.JavaScript;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace DPBack.Infrastructure.Seeder;

public static class UserSeeder
{
    public static async Task SeedAsync(UserStoreDbContext context, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
    {
        if (!context.Users.Any(x => x.Login == "admin"))
        {
            var admin = new User(Guid.NewGuid(), "admin", "", "admin@local", UserRole.Admin, DateTime.UtcNow);
            var password = passwordHasher.HashPassword(admin, configuration["Accounts:BaseAdminPassword"]??"xxxx");
            var entity = new UserEntity(admin.Id, admin.Login, password, admin.Email, admin.Role, admin.CreatedAt);
            context.Users.Add(entity);
        }

        if (!context.Users.Any(x => x.Login == "worker1"))
        {
            var worker = new User(Guid.NewGuid(), "worker1", "", "worker1@local", UserRole.Worker, DateTime.UtcNow);
            var password = passwordHasher.HashPassword(worker, configuration["Accounts:BaseWorkerPassword"]??"xxxx");
            var entity = new UserEntity(worker.Id, worker.Login, password, worker.Email, worker.Role, worker.CreatedAt);
            context.Users.Add(entity);
        }

        await context.SaveChangesAsync();
    }
}