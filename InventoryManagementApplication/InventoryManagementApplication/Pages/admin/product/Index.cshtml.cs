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
		private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

		public IndexModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context, ProductManager manager)
		{
			_context = context;
			_manager = manager;
		}
		public IList<Product> Products { get; set; } = default!;

		public async Task OnGet()
		{
			Products = await _manager.GetAllProductsAsync();
			
		}
	}
}
