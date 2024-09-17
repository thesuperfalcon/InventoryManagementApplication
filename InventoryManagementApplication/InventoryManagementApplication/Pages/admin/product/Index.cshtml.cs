using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InventoryManagementApplication.Pages.admin.product
{
	public class IndexModel : PageModel
	{
		private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

		public IndexModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
		{
			_context = context;
		}
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public IList<Product> Products { get; set; } = default!;

		public async Task OnGetAsync()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;


				HttpResponseMessage responseProducts = await client.GetAsync("api/Products");
				if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();
					List<Models.Product> products = JsonSerializer.Deserialize<List<Models.Product>>(responseString);
					Products = products.ToList();

				}			
			}
		}
	}
}
