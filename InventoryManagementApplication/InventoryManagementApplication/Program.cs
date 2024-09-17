using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagementApplication
{
    public class Program
    {
        public static async Task Main(string[] args)  // Gör metoden till asynkron
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("InventoryManagementApplicationContextConnection") ?? throw new InvalidOperationException("Connection string 'InventoryManagementApplicationContextConnection' not found.");

            builder.Services.AddDbContext<InventoryManagementApplicationContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<InventoryManagementUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = false;

            })
            .AddRoles<InventoryManagementRole>()  //Lägger till roller
            .AddEntityFrameworkStores<InventoryManagementApplicationContext>();

            // Lägger till Admin-policy
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            //Lägger till rollen för Admin
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<InventoryManagementRole>>();
                var userManager = services.GetRequiredService<UserManager<InventoryManagementUser>>();
                await SeedRolesAndAdminUser(roleManager, userManager);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            await app.RunAsync();
        }

        private static async Task SeedRolesAndAdminUser(RoleManager<InventoryManagementRole> roleManager, UserManager<InventoryManagementUser> userManager)
        {
            var roles = new[] { "Admin" };

            // Skapa roller
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var newRole = new InventoryManagementRole
                    {
                        Name = role,
                        NormalizedName = role.ToUpper(),
                        FullAccess = (role == "Admin"),
                        RoleName = role
                    };
                    await roleManager.CreateAsync(newRole);

                }
            }

            // Om admin inte finns, skapa admin vid start
            var adminUserName = "adminUser";
            var adminPassword = "Admin123!";

            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                adminUser = new InventoryManagementUser
                {
                    UserName = adminUserName,
                    EmployeeNumber = "0000",
                    FirstName = "Admin",
                    LastName = "User"
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}