using DPBack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DPBack.Infrastructure.Contexts;

public class ProductStoreDbContext:DbContext
{
    public ProductStoreDbContext(DbContextOptions<ProductStoreDbContext> options) :base(options)
    {
        
    }
    public DbSet<BusinesscardEntity> Businesscards {get;set;}
    public DbSet<BusinesscardFinishTypeEntity> BusinesscardCoating { get; set; }
    public DbSet<BusinesscardPaperTypeEntity> BusinessCardPaper { get; set; }
}