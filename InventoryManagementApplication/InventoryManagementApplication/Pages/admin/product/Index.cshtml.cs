using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
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
			Products = await _manager.GetAllProductsAsync();
			
		}
	}
}
