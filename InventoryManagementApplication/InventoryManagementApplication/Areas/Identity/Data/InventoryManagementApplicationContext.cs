using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Data;

public class InventoryManagementApplicationContext : IdentityDbContext<InventoryManagementUser>
{
    public InventoryManagementApplicationContext(DbContextOptions<InventoryManagementApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Storage> Storages { get; set; }
    public DbSet<InventoryTracker> InventoryTracker { get; set; }
    public DbSet<ActivityLog> ActivityLog { get; set; }
    public DbSet<Statistic> Statistics { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
   
        builder.Entity<InventoryManagementRole>()
                  .Property(r => r.RoleName)
                  .HasMaxLength(256);

        builder.Entity<InventoryManagementRole>()
               .Property(r => r.FullAccess)
               .HasDefaultValue(false);

        builder.Entity<Statistic>()
            .HasOne(x => x.InitialStorage)
            .WithMany(x => x.Statistics)
            .HasForeignKey(x => x.InitialStorageId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Statistic>()
            .HasOne(x => x.DestinationStorage)
            .WithMany()
            .HasForeignKey(x => x.DestinationStorageId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Statistic>()
            .HasOne(x => x.InitialStorage)
            .WithMany(x => x.Statistics)
            .HasForeignKey(x => x.InitialStorageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Statistic>()
            .HasOne(x => x.DestinationStorage)
            .WithMany()
            .HasForeignKey(x => x.DestinationStorageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

