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

        public ProductDetailsModel(LogManager logManager, UserManager userManager, ProductManager productManager)
        {
            _productManager = productManager;

            _logManager = logManager;
            _userManager = userManager;
            _productManager = productManager;
        }

        [BindProperty]
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
		public async Task<IActionResult> OnPostUpdateProductInfoAsync(int productId, string productName, string productDescription)
		{
			if (!ModelState.IsValid)
			{
				return Page(); // Return the page with validation errors
			}

			var productToUpdate = await  _productManager.GetProductByIdAsync(productId, null);
			if (productToUpdate == null)
			{
				return NotFound();
			}

			productToUpdate.Name = productName;
			productToUpdate.Description = productDescription;

            await _productManager.EditProductAsync(productToUpdate);

			return RedirectToPage(new { id = Product.Id });
		}
	}
}
