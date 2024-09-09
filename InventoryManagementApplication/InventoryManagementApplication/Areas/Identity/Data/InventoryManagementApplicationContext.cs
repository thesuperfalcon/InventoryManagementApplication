using InventoryManagementApplication.Areas.Identity.Data;
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<InventoryManagementRole>()
                  .Property(r => r.RoleName)
                  .HasMaxLength(256);

        builder.Entity<InventoryManagementRole>()
               .Property(r => r.FullAccess)
               .HasDefaultValue(false); // Default value for FullAccess
    }
}

