using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client.Extensions.Msal;

namespace InventoryManagementApplication.Pages.admin.tracker
{
	public class CreateModel : PageModel
	{
		private readonly TrackerManager _trackerManager;
		private readonly ProductManager _productManager;
		private readonly StorageManager _storageManager;

		public CreateModel(TrackerManager trackerManager, ProductManager productManager, StorageManager storageManager)
		{
			_trackerManager = trackerManager;
			_productManager = productManager;
			_storageManager = storageManager;
		}
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
			var products = await _productManager.GetAllProductsAsync();
			var storages = await _storageManager.GetAllStoragesAsync();

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

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			int quantity = 0;
			var existingTrackerSearcher = await _trackerManager.GetAllTrackersAsync();

			var existingTracker = existingTrackerSearcher.Where(x => x.Product.Id == InventoryTracker.ProductId && x.Storage.Id ==
					InventoryTracker.StorageId).FirstOrDefault();

			if (existingTracker == null)
			{
				//var product = await _productManager.GetOneProductAsync(InventoryTracker.ProductId);
				//var storage = await _storageManager.GetOneStorageAsync(InventoryTracker.StorageId);
				//InventoryTracker.Storage = storage;
				//InventoryTracker.Product = product;
				await _trackerManager.CreateTrackerAsync(InventoryTracker);
				quantity = (int)InventoryTracker.Quantity;
				
			}
			else
			{
				quantity = (int)InventoryTracker.Quantity;
			}

			var inventoryTrackers1 = await _productManager.GetAllProductsAsync();
			Product = inventoryTrackers1
				.Where(x => x.Id == InventoryTracker.ProductId)
				.FirstOrDefault();

			var inventoryTrackers2 = await _storageManager.GetAllStoragesAsync();
			Storage = inventoryTrackers2
				.Where(x => x.Id == InventoryTracker.StorageId)
				.FirstOrDefault();

			if (Storage != null)
			{
				CurrentSpace = Storage.MaxCapacity - Storage.CurrentStock;

				if (CurrentSpace < quantity)
				{

					StatusMessage = $"Finns ej plats i {Storage.Name}. Välj annan lagerplats!";
					if (existingTracker == null)
					{
						await _trackerManager.DeleteTrackerAsync(InventoryTracker.Id);						
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
				return RedirectToPage("./Create");
			}
			//Kolla om den här är felplacerad?
			if (InventoryTracker != null && InventoryTracker.Id != 0)
			{
				await _trackerManager.DeleteTrackerAsync(InventoryTracker.Id);
				return RedirectToPage("./Create");

			}
			else if (CurrentSpace < quantity)
			{
				StatusMessage = $"Finns ej plats i {Storage.Name}. Välj annan lagerplats!";
				return RedirectToPage("./Create");
			}
			if (existingTracker != null)
			{
				existingTracker.Quantity += InventoryTracker.Quantity;
				await _trackerManager.EditTrackerAsync(existingTracker);
			}

			var updatedStorage = new Models.Storage
			{
				Id = Storage.Id,
				Name = Storage.Name,
				Created = Storage.Created,
				CurrentStock = Storage.CurrentStock + quantity,
				MaxCapacity = Storage.MaxCapacity,
				Updated = DateTime.Now
			};

			var updatedProduct = new Models.Product
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

			await _productManager.EditProductAsync(updatedProduct);
			await _storageManager.EditStorageAsync(updatedStorage);
			

			return RedirectToPage("./Index");
		}
	}
}


