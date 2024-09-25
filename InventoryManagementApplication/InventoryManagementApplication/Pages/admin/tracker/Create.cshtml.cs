using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Helpers;
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
		private readonly SelectListHelpers _selectListHelpers;
        private readonly ProductMovementHelpers _productMovementHelpers;

		public CreateModel(TrackerManager trackerManager, ProductManager productManager, StorageManager storageManager, SelectListHelpers selectListHelpers, ProductMovementHelpers productMovementHelpers)
		{
			_trackerManager = trackerManager;
			_productManager = productManager;
			_storageManager = storageManager;
			_selectListHelpers = selectListHelpers;
            _productMovementHelpers = productMovementHelpers;
		}
		[TempData]
		public string StatusMessage { get; set; }
		public SelectList StorageSelectList { get; set; }
		public SelectList ProductSelectList { get; set; }

		[BindProperty]
		public InventoryTracker InventoryTracker { get; set; } = default!;
        [BindProperty]
		public Models.Storage DefaultStorage { get; set; }
		public Models.Storage Storage { get; set; }
		public Product? Product { get; set; }
		public int? CurrentSpace { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			var products = await _productManager.GetProductsAsync(false);
			var storages = await _storageManager.GetStoragesAsync(false);

			DefaultStorage = await _storageManager.GetStorageByIdAsync(44, null);

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

			StorageSelectList = await _selectListHelpers.GenerateStorageSelectListAsync(DefaultStorage.Id);
			ProductSelectList = await _selectListHelpers.GenerateProductSelectListBasedOfStorageAsync(DefaultStorage.Id);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			int productId = (int)InventoryTracker.ProductId;
			int fromStorageId = (int)DefaultStorage.Id;
			int toStorageId = (int)InventoryTracker.StorageId;
			int quantity = (int)InventoryTracker.Quantity;

            if (productId < 0 || fromStorageId < 0 || toStorageId < 0 || quantity < 0)
            {
                return RedirectToPage("./Create", new { id = InventoryTracker.Id });
            }

            bool status = false;

            var tuple = await _productMovementHelpers.MoveProductAsync(productId, fromStorageId, toStorageId, quantity);
            if (tuple != null)
            {
                status = tuple.Item1;
                if (status == false)
                {
                    StatusMessage = "Förflyttning lyckades ej";
                    return RedirectToPage("./Create", new { id = InventoryTracker.Id });
                }
                else
                {
                    StatusMessage = tuple.Item2;
                    return RedirectToPage("./Index");
                }
            }
            StatusMessage = "Förflyttning lyckades ej";
            return RedirectToPage("./Create", new { id = InventoryTracker.Id });

            //         var product = await _productManager.GetProductByIdAsync(InventoryTracker.ProductId, null);
            //         var storage = await _storageManager.GetStorageByIdAsync(InventoryTracker.StorageId, null);

            //if(product == null || storage == null)
            //{
            //	StatusMessage = "Produkt eller lagret finns ej";
            //	return RedirectToPage("./Create");
            //}

            //         int quantity = (int)InventoryTracker.Quantity;

            //if(storage.CurrentStock + quantity > storage.MaxCapacity)
            //{
            //	StatusMessage = $"Finns ej plats i {storage.Name}. Välj annan lagerplats.";
            //             return RedirectToPage("./Create");
            //         }

            //         if (quantity > product.CurrentStock)
            //         {

            //             StatusMessage = $"Antalet produkter du vill lägga till finns ej tillgänglig. Antal produkter utan lager: {product.CurrentStock}";
            //             return RedirectToPage("./Create");
            //         }

            //InventoryTracker.Storage = storage;
            //InventoryTracker.Product = product;


            ////skapa dal för detta 
            //         var existingTrackerSearcher = await _trackerManager.GetAllTrackersAsync();

            //var existingTracker = existingTrackerSearcher.Where(x => x.Product.Id == InventoryTracker.ProductId && x.Storage.Id ==
            //		InventoryTracker.StorageId).FirstOrDefault();

            //if (existingTracker != null)
            //{
            //	existingTracker.Quantity += quantity;
            //             await _trackerManager.EditTrackerAsync(existingTracker);
            //         }
            //         else
            //{
            //	await _trackerManager.CreateTrackerAsync(InventoryTracker);
            //         }

            //await _productManager.EditProductAsync(new Product
            //{
            //             Id = product.Id,
            //             Name = product.Name,
            //             Description = product.Description,
            //             ArticleNumber = product.ArticleNumber,
            //             Price = product.Price,
            //             TotalStock = product.TotalStock,
            //             Created = product.Created,
            //             CurrentStock = product.CurrentStock - quantity,
            //             Updated = DateTime.Now
            //         });

            //await _storageManager.EditStorageAsync(new Models.Storage
            //{
            //	Id = storage.Id,
            //	Name = storage.Name,
            //	Created = storage.Created,
            //	CurrentStock = storage.CurrentStock + quantity,
            //	MaxCapacity = storage.MaxCapacity,
            //	Updated = DateTime.Now
            //});

            //return RedirectToPage("./Index");
        }
	}
}


