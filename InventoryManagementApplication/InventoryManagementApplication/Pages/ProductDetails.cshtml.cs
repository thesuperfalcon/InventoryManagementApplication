using InventoryManagementApplication.Models;
using InventoryManagementApplication.DAL;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using InventoryManagementApplication.Data;


namespace InventoryManagementApplication.Pages
{
public class ProductDetailsModel : PageModel
    {
        private readonly InventoryManagementApplicationContext _context;
        private readonly LogManager _logManager;
        private readonly UserManager _userManager;

        public ProductDetailsModel(InventoryManagementApplicationContext context, LogManager logManager, UserManager userManager)
        {
            _context = context;
            _logManager = logManager;
            _userManager = userManager;
        }

        public Product Product { get; set; }
        public List<Log> ActivityLogs { get; set; } = new List<Log>();
        public List<string> UserFullName { get; set; } = new List<string>();
        public List<string> UserEmployeeNumbers { get; set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Hämta produkten
            Product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            // Hämta alla loggar och filtrera efter produkten
            var allLogs = await _logManager.GetAllLogsAsync();
            ActivityLogs = allLogs.Where(log => log.EntityType.Contains("Product") && log.EntityName == Product.Name).ToList();

            // Hämta användarinformation för varje logg
           //Denna hämtar icke-raderade användare
            var users = await _userManager.GetAllUsersAsync(null);
           
           //Denna hämtar alla: var users = await _userManager.GetAllUsersAsync(null);
            //denna hämtar endast raderade användare: var users = await _userManager.GetAllUsersAsync(true);

            var userDictinary = users.ToDictionary(
                u => u.Id,
                u => new { FullName = $"{u.LastName} {u.FirstName}", EmployeeNumber = u.EmployeeNumber });

            foreach (var log in ActivityLogs)
            {
                if (userDictinary.TryGetValue(log.UserId, out var userInfo))
                {
                    UserFullName.Add(userInfo.FullName);
                    UserEmployeeNumbers.Add(userInfo.EmployeeNumber);
                }
                else
                {
                    UserFullName.Add("Unknown User");
                    UserEmployeeNumbers.Add("N/A");
                }
            }

            return Page();
        }
    
        public async Task<IActionResult> OnPostUpdateProductInfoAsync(int ProductId, string ProductName, string ProductDescription /*, int StorageId*/)
        {
            var product = await _context.Products
                .Include(p => p.InventoryTrackers)
                .FirstOrDefaultAsync(p => p.Id == ProductId);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = ProductName;
            product.Description = ProductDescription;

            /* Commenting out product movement related logic */
            // var tracker = product.InventoryTrackers.FirstOrDefault();
            // if (tracker != null)
            // {
            //     tracker.StorageId = StorageId;
            // }
            // else
            // {
            //     product.InventoryTrackers.Add(new InventoryTracker
            //     {
            //         ProductId = ProductId,
            //         StorageId = StorageId,
            //         Quantity = product.CurrentStock,
            //         Modified = DateTime.Now
            //     });
            // }

            

            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = ProductId });
        }
    }
}
