using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using InventoryManagementApplication.Helpers;

using System.Text.Json.Serialization;
using InventoryManagementApplication.DAL;
namespace InventoryManagementApplication
{
    public class Program
    {     
        public static async Task Main(string[] args)  // G�r metoden till asynkron
        {          
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("InventoryManagementApplicationContextConnection") ?? throw new InvalidOperationException("Connection string 'InventoryManagementApplicationContextConnection' not found.");

            builder.Services.AddDbContext<InventoryManagementApplicationContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddTransient<Models.Product>();
            builder.Services.AddScoped<StatisticManager>();
            builder.Services.AddScoped<DAL.StatisticManager>();
            builder.Services.AddScoped<DAL.LogManager>();
            builder.Services.AddScoped<InventoryManagementApplication.DAL.UserManager>();
			builder.Services.AddScoped<UserManager<InventoryManagementUser>>();
			builder.Services.AddScoped<RoleManager<InventoryManagementRole>>();
            builder.Services.AddScoped<DAL.RoleManager>();
            builder.Services.AddScoped<Helpers.ManageAccountHelpers>();
            builder.Services.AddHttpContextAccessor();

			//builder.Services.AddTransient<Models.Statistic>();
			//builder.Services.AddTransient<Models.StatisticDto>();

			//builder.Services.AddControllers()
			//.AddJsonOptions(options =>
			//{
			//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
			//});

			//var supportedCultures = new[] { new CultureInfo("en-US") };

			var supportedCultures = new[] { new CultureInfo("sv-SE"), new CultureInfo("en-US") };
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("sv-SE");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            builder.Services.AddDefaultIdentity<InventoryManagementUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = false;

            })
            .AddRoles<InventoryManagementRole>()  //L�gger till roller
            .AddEntityFrameworkStores<InventoryManagementApplicationContext>();

            // L�gger till Admin-policy
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddScoped<SelectListHelpers>();
            builder.Services.AddScoped<StatisticLeaderboardHelpers>();
            builder.Services.AddScoped<ProductMovementHelpers>();
            builder.Services.AddScoped<ProductManager>();
            builder.Services.AddScoped<StorageManager>();
            builder.Services.AddScoped<TrackerManager>();

            builder.Services.AddControllersWithViews()
            .AddJsonOptions(options =>
             options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);

            // Cookies
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var app = builder.Build();

            // Cookies
            app.UseCookiePolicy();

            //L�gger till rollen f�r Admin
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<InventoryManagementRole>>();
                var userManager = services.GetRequiredService<UserManager<InventoryManagementUser>>();
                //await SeedRolesAndAdminUser(roleManager, userManager);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRequestLocalization();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            await app.RunAsync();
        }      
    }
}