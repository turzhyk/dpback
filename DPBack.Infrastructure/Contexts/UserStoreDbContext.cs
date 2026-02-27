
using DPBack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DPBack.Infrastructure.Contexts;

public class UserStoreDbContext : DbContext
{
    public UserStoreDbContext(DbContextOptions<UserStoreDbContext> options) : base(options)
    {
    }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserAdressEntity> Adresses { get; set; }
}