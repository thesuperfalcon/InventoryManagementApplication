using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

		public async Task OnGet()
		{
			//ändra till false
			Products = await _manager.GetProductsAsync(null);
<<<<<<< HEAD
		}

		public IActionResult OnPost()
		{
			return RedirectToPage("./Create");
		}
	}
=======

        }
    }
>>>>>>> f47cc9066a29aec6060a9703804fe0cc9d081d19
}
