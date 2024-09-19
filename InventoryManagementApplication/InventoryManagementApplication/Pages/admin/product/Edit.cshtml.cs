using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.Pages.admin.product
{
	public class EditModel : PageModel
	{
		private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public EditModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
		{
			_context = context;
		}

		[TempData]
		public string StatusMessage { get; set; }
		[BindProperty]
		public Product Product { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			var products = new List<Product>();
			var product = new Product();

			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;

				if (id == null)
				{
					return NotFound();
				}
				HttpResponseMessage responseProducts = await client.GetAsync("api/Products");
				if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();
					products = System.Text.Json.JsonSerializer.Deserialize<List<Models.Product>>(responseString);

				}
				product = products.Where(p => p.Id == id).FirstOrDefault();
				//var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
				if (product == null)
				{
					return NotFound();
				}
			}

			Product = product;
			return Page();
		}

		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more information, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			var inventoryTrackers = new List<InventoryTracker>();
			if (!ModelState.IsValid)
			{
				return Page();
			}
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;

				HttpResponseMessage responseInventoryTracker = await client.GetAsync("api/InventoryTrackers");
				{
					if (responseInventoryTracker.IsSuccessStatusCode)
					{
						string responseString = await responseInventoryTracker.Content.ReadAsStringAsync();
						inventoryTrackers = System.Text.Json.JsonSerializer.Deserialize<List<Models.InventoryTracker>>(responseString).Where(p => p.Id == Product.Id).ToList();
					}

				}

				//inventoryTrackers = inventoryTrackers.Where(x => x.Id == Product.Id).ToList();
			    //inventoryTrackers = await _context.InventoryTracker.Where(x => x.ProductId == Product.Id).ToListAsync();

				var productQuantity = inventoryTrackers.Sum(x => x.Quantity);

				if (inventoryTrackers != null && inventoryTrackers.Count > 0)
				{

					if (Product.TotalStock < productQuantity)
					{
						StatusMessage = $"Går ej att ändra. Currentstock: {Product.CurrentStock} Total: {Product.TotalStock}";
						return RedirectToPage("./Edit", new { id = Product.Id });
					}
					else
					{
						var input = Product.TotalStock - productQuantity;
						Product.CurrentStock = input;
					}
				}
				else
				{
					Product.CurrentStock = Product.TotalStock;
				}
				//Konvertera till API????
				_context.Attach(Product).State = EntityState.Modified;

				try
				{
					var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(Product),Encoding.UTF8,"application/json");
					HttpResponseMessage response = await client.PutAsync($"api/products/{Product.Id}", content);

					if (!response.IsSuccessStatusCode)
					{
						string responseContent = await response.Content.ReadAsStringAsync();
						StatusMessage = $"Failed to update product. Status: {response.StatusCode}, Reason: {response.ReasonPhrase}, Details: {responseContent}";
						return RedirectToPage("./Edit", new { id = Product.Id });
					}
					//await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductExists(Product.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}

				return RedirectToPage("./Index");
			}

			
		}
		private bool ProductExists(int id)
		{
			return _context.Products.Any(e => e.Id == id);
		}
	}
	}
