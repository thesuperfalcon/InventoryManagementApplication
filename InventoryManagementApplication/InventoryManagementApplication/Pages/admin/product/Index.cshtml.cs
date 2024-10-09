using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static InventoryManagementApplication.Pages.UsersRolesModel;

namespace InventoryManagementApplication.Pages.admin.product
{
	public class IndexModel : PageModel
	{
		private readonly ProductManager _manager;

		public IndexModel(ProductManager manager)
		{
			_manager = manager;
		}
		public IList<Product> Products { get; set; } = default!;
		[BindProperty]
		public bool IsDeletedToggle { get; set; }
		public int ProductCount { get; set; }

		public async Task OnGet()
		{
            Products = await LoadProducts(IsDeletedToggle);
			ProductCount = Products.Count();         
		}

		public async Task<IActionResult> OnPostAsync(int buttonId)
		{
			if(buttonId == 1)
			{
				IsDeletedToggle = !IsDeletedToggle;
				Products = await LoadProducts(IsDeletedToggle);
                ProductCount = Products.Count();
                return Page();
			}
			else
			{
                return RedirectToPage("./Create");
            }
			
		}
        private async Task<IList<Product>> LoadProducts(bool isDeleted)
        {
            var products = await _manager.GetProductsAsync(isDeleted);
                      
            return products;
        }
    }

}
