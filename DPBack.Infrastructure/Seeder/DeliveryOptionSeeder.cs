using DPBack.Application.Abstractions;
using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;

namespace DPBack.Infrastructure.Seeder;

public static class DeliveryOptionSeeder
{
    public static async Task SeedAsync(OrderStoreDbContext context)
    {
        if (context.DeliveryOptions.Any(x => x.Title == "Paczkomat InPost"))
        {
            context.DeliveryOptions.Add(new DeliveryTypeEntity
                { Id = Guid.NewGuid(), Title = "Paczkomat InPost", Price = 12.99m });
        }
    }
}