using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace InventoryManagementApplication.Data;

  public class InventoryManagementApplicationContext : IdentityDbContext<InventoryManagementUser, InventoryManagementRole, string> 
    {
    public InventoryManagementApplicationContext(DbContextOptions<InventoryManagementApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Storage> Storages { get; set; }
    public DbSet<InventoryTracker> InventoryTracker { get; set; }
    public DbSet<Statistic> Statistics { get; set; }
    public DbSet<Log> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
       
       builder.Entity<Log>()
            .HasKey(e => e.Id);

        builder.Entity<InventoryTracker>()
            .HasOne(s => s.Product)
            .WithMany(s => s.InventoryTrackers)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<InventoryTracker>()
            .HasOne(s => s.Storage)
            .WithMany(s => s. InventoryTrackers)
            .HasForeignKey(s => s.StorageId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Storage>()
            .HasMany(s => s.InventoryTrackers)
            .WithOne(it => it.Storage)
            .HasForeignKey(it => it.StorageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

