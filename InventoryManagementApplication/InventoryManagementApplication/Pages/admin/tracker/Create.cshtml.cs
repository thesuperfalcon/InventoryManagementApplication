using Azure;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.Pages.admin.tracker
{
	public class CreateModel : PageModel
	{
		private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

		public CreateModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
		{
			_context = context;
		}
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		[TempData]
		public string StatusMessage { get; set; }
		public SelectList StorageSelectList { get; set; }
		public SelectList ProductSelectList { get; set; }

		[BindProperty]
		public InventoryTracker InventoryTracker { get; set; } = default!;
		public Models.Storage Storage { get; set; }
		public Product? Product { get; set; }
		public int? CurrentSpace { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				var products = new List<Models.Product>();
				var storages = new List<Models.Storage>();

				HttpResponseMessage responseProducts = await client.GetAsync($"api/Products/");
				if (responseProducts.IsSuccessStatusCode)
				{

					string responseString = await responseProducts.Content.ReadAsStringAsync();
					products = System.Text.Json.JsonSerializer.Deserialize<List<Models.Product>>(responseString);
					Console.WriteLine(responseString);
				}

				HttpResponseMessage responseStorage = await client.GetAsync($"api/Storages/");
				if (responseStorage.IsSuccessStatusCode)
				{
					string responseString = await responseStorage.Content.ReadAsStringAsync();
					storages = JsonSerializer.Deserialize<List<Models.Storage>>(responseString);
					Console.WriteLine(responseStorage);
				}
				Console.WriteLine(products.Count); // Log the product count
				Console.WriteLine(storages.Count); // Log the storage count

				//return RedirectToPage("./Index");
				//Allt returnerar null, får inte upp några alternativ på Create Tracker
				//var storages = await _context.Storages.ToListAsync();
				//var products = await _context.Products.ToListAsync();

				var productItems = products.Select(x => new
				{
					Value = x.Id,
					Text = $"{x.Name} (Antal utan lager: {x.CurrentStock})"
				});

				var storageItems = storages.Select(x => new
				{
					Value = x.Id.ToString(),
					Text = $"{x.Name} (Lediga platser: {x.MaxCapacity - x.CurrentStock})"
				}).ToList();

				StorageSelectList = new SelectList(storageItems, "Value", "Text");
				ProductSelectList = new SelectList(productItems, "Value", "Text");

			}
			return Page();
		}


		// To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
		public async Task<IActionResult> OnPostAsync()
		{

			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;

				if (!ModelState.IsValid)
				{
					return Page();
				}
				var existingTracker = new InventoryTracker();
				var products = new List<Models.Product>();

				int quantity = 0;
				HttpResponseMessage responseTrackers = await client.GetAsync($"api/InventoryTrackers/");
				if (responseTrackers.IsSuccessStatusCode)
				{
					string responseString = await responseTrackers.Content.ReadAsStringAsync();
					existingTracker = JsonSerializer.Deserialize<List<Models.InventoryTracker>>(responseString).Where(x => x.ProductId == InventoryTracker.ProductId && x.StorageId ==
					InventoryTracker.StorageId).FirstOrDefault();

				}

				//var existingTracker = await _context.InventoryTracker.Where(x => x.ProductId == InventoryTracker.ProductId && x.StorageId == InventoryTracker.StorageId).FirstOrDefaultAsync();
				if (existingTracker == null)
				{
					var json = JsonSerializer.Serialize(InventoryTracker);

					//Gör det möjligt att skicka innehåll till API
					StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PostAsync("api/InventoryTrackers/", httpContent);				
					quantity = (int)InventoryTracker.Quantity;
                    //await _context.SaveChangesAsync();
                    //_context.InventoryTracker.Add(InventoryTracker);
                }
                else
				{
					quantity = (int)InventoryTracker.Quantity;
				}

				HttpResponseMessage responseProducts = await client.GetAsync($"api/InventoryTrackers/");
                string responseBody = await responseProducts.Content.ReadAsStringAsync();
                Console.WriteLine($"Status Code: {responseProducts.StatusCode}");
                Console.WriteLine($"Response Body: {responseBody}");
                if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();
					Product = JsonSerializer.Deserialize<List<InventoryTracker>>(responseString).Select(x => x.Product)
					.Where(x => x.Id == InventoryTracker.ProductId)
					.FirstOrDefault();
				}

				HttpResponseMessage responseStorage = await client.GetAsync($"api/InventoryTrackers/");
				if (responseStorage.IsSuccessStatusCode)
				{
					string responseString = await responseStorage.Content.ReadAsStringAsync();
					Storage = JsonSerializer.Deserialize<List<InventoryTracker>>(responseString).Select(x => x.Storage)
                    .Where(x => x.Id == InventoryTracker.StorageId)
                    .FirstOrDefault();
                }

                //Storage = await _context.InventoryTracker
                //   .Select(x => x.Storage)
                //.Where(x => x.Id == InventoryTracker.StorageId)
                //.FirstOrDefaultAsync();

                //Product = await _context.InventoryTracker
                //.Select(x => x.Product)
                //.Where(x => x.Id == InventoryTracker.ProductId)
                //.FirstOrDefaultAsync();

                if (Storage != null)
                {
                    CurrentSpace = Storage.MaxCapacity - Storage.CurrentStock;

                    if (CurrentSpace < quantity)
                    {

                        StatusMessage = $"Finns ej plats i {Storage.Name}. Välj annan lagerplats!";
                        if (existingTracker == null)
                        {
                            var json = JsonSerializer.Serialize(InventoryTracker);

                            //Gör det möjligt att skicka innehåll till API
                            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PostAsync("api/InventoryTrackers/", httpContent);
                            //_context.Remove(InventoryTracker);
                            //await _context.SaveChangesAsync();
                        }
                        return RedirectToPage("./Create");

                    }
                }
                else
                {
                    StatusMessage = "Storage not found.";
                    return RedirectToPage("./Create");
                }

                


				if (quantity > Product.CurrentStock)
				{
					int id = InventoryTracker.Id;

					StatusMessage = $"Antalet produkter du vill lägga till finns ej tillgänglig. Antal produkter utan lager: {Product.CurrentStock}";
					if (InventoryTracker != null && InventoryTracker.Id != 0)
					{
						var json = JsonSerializer.Serialize(InventoryTracker);

						//Gör det möjligt att skicka innehåll till API
						StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
						HttpResponseMessage response = await client.DeleteAsync($"api/InventoryTrackers/{id}");
						//_context.InventoryTracker.Remove(InventoryTracker);
						//await _context.SaveChangesAsync();
					}
					return RedirectToPage("./Create");
				}
				//Ändrade Storage.CurrentStock till currentSpace
				else if (CurrentSpace < quantity)
				{
					StatusMessage = $"Finns ej plats i {Storage.Name}. Välj annan lagerplats!";
                    return RedirectToPage("./Create");
                }

				if (existingTracker != null)
				{
					existingTracker.Quantity += InventoryTracker.Quantity;
				}
                var updatedStorage = new
                {
                    Id = Storage.Id,
					Name = Storage.Name,
					Created = Storage.Created,
                    CurrentStock = Storage.CurrentStock + quantity,
					MaxCapacity = Storage.MaxCapacity,					
                    Updated = DateTime.Now
                };

                var updatedProduct = new
                {
                    Id = Product.Id,
					Name = Product.Name,
					Description = Product.Description,
					ArticleNumber = Product.ArticleNumber,
					Price = Product.Price,
					TotalStock = Product.TotalStock,
					Created = Product.Created,
                    CurrentStock = Product.CurrentStock - quantity,
                    Updated = DateTime.Now
                };

                var productJson = JsonSerializer.Serialize(updatedProduct);
                var productContent = new StringContent(productJson, Encoding.UTF8, "application/json");
                await client.PutAsync($"api/products/{updatedProduct.Id}", productContent);

                // Send the updated storage data to the API
                var storageJson = JsonSerializer.Serialize(updatedStorage);
                var storageContent = new StringContent(storageJson, Encoding.UTF8, "application/json");
                await client.PutAsync($"api/storages/{updatedStorage.Id}", storageContent);

                // Send the updated product data to the API
               
                //Storage.CurrentStock += quantity;
                //Product.CurrentStock -= quantity;
                //Storage.Updated = DateTime.Now;
                //await _context.SaveChangesAsync();

            }
			return RedirectToPage("./Index");
		}
	}
}
