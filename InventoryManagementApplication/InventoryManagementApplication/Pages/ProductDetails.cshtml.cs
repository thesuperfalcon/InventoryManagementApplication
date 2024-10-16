using InventoryManagementApplication.Models;
using InventoryManagementApplication.DAL;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementApplication.Pages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly LogManager _logManager;
        private readonly UserManager _userManager;
        private readonly ProductManager _productManager;

        public ProductDetailsModel(InventoryManagementApplicationContext context, LogManager logManager, UserManager userManager, ProductManager productManager)
        {
            _productManager = productManager;

            _logManager = logManager;
            _userManager = userManager;
            _productManager = productManager;
        }

        public Product Product { get; set; }
        public List<Log> ActivityLogs { get; set; } = new List<Log>();
        public List<string> UserFullName { get; set; } = new List<string>();
        public List<string> UserEmployeeNumbers { get; set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _productManager.GetProductByIdAsync(id, null);

            if (Product == null)
            {
                return NotFound();
            }

			ActivityLogs = await _logManager.GetLogByForEntityAsync("Product", id);

            return Page();
        }
    }
}