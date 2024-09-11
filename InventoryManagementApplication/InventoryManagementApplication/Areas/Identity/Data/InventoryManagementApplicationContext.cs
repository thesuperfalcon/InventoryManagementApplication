using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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

        builder.Entity<ActivityLog>()
                   .HasOne(al => al.User)
                   .WithMany(u => u.ActivityLogs)
                   .HasForeignKey(al => al.UserId)
                   .OnDelete(DeleteBehavior.SetNull);

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

        builder.Entity<Statistic>()
            .HasOne(s => s.Reporter)
            .WithMany(u => u.StatisticReporters)
            .HasForeignKey(s => s.ReporterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Statistic>()
            .HasOne(s => s.Executer)
            .WithMany(u => u.StatisticExecuters)
            .HasForeignKey(s => s.ExecuterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Statistic>()
            .HasOne(s => s.InitialStorage)
            .WithMany(s => s.StatisticInitialStorages)
            .HasForeignKey(s => s.InitialStorageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Statistic>()
            .HasOne(s => s.DestinationStorage)
            .WithMany(s => s.StatisticDestinationStorages)
            .HasForeignKey(s => s.DestinationStorageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Storage>()
            .HasMany(s => s.InventoryTrackers)
            .WithOne(it => it.Storage)
            .HasForeignKey(it => it.StorageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Storage>()
            .HasMany(s => s.StatisticDestinationStorages)
            .WithOne(s => s.DestinationStorage)
            .HasForeignKey(s => s.DestinationStorageId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Storage>()
            .HasMany(s => s.StatisticInitialStorages)
            .WithOne(s => s.InitialStorage)
            .HasForeignKey(s => s.InitialStorageId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

