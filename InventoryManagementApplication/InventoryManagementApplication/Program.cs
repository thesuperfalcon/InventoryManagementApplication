using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Areas.Identity.Data;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
namespace InventoryManagementApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("InventoryManagementApplicationContextConnection") ?? throw new InvalidOperationException("Connection string 'InventoryManagementApplicationContextConnection' not found.");

            builder.Services.AddDbContext<InventoryManagementApplicationContext>(options => options.UseSqlServer(connectionString));


            var supportedCultures = new[] { new CultureInfo("en-US") };
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            builder.Services.AddDefaultIdentity<InventoryManagementUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = false;
                
            })
            .AddEntityFrameworkStores<InventoryManagementApplicationContext>();

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

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

            app.Run();
        }
    }
}
