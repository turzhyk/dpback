using DPBack.Domain.Models;
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
        
        public DbSet<DeliveryTypeEntity> DeliveryOptions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("OrderNumbers")
                .StartsAt(10001)
                .IncrementsBy(1);

            modelBuilder.Entity<OrderEntity>()
                .Property(o => o.OrderNumber)
                .HasDefaultValueSql("nextval('\"OrderNumbers\"')");
        }
    }
}
