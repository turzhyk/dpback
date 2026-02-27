using DPBack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;


namespace DPBack.Infrastructure.Contexts
{
    public class OrderStoreDbContext : DbContext
    {
        public OrderStoreDbContext(DbContextOptions<OrderStoreDbContext> options):base(options)
        {
            
        }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<OrderHistoryElementEntity> OrderStatusHistories { get; set; }

    }
}
