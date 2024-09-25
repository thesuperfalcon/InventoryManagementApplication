using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagementApplication.Pages.admin.tracker
{
	public class DeleteModel : PageModel
	{
		private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;
		private readonly TrackerManager _trackerManager;
		private readonly ProductManager _productManager;
		private readonly StorageManager _storageManager;

		public DeleteModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context,
			TrackerManager trackerManager, ProductManager productManager, StorageManager storageManager)
		{
			_context = context;
			_trackerManager = trackerManager;
			_productManager = productManager;
			_storageManager = storageManager;
		}

		[BindProperty]
		public InventoryTracker InventoryTracker { get; set; } = default!;


		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var inventoryTracker = await _trackerManager.GetOneTrackerAsync((int)id);
			// var inventorytracker = await _context.InventoryTracker.FirstOrDefaultAsync(m => m.Id == id);

			if (inventoryTracker == null)
			{
				return NotFound();
			}
			else
			{
				InventoryTracker = inventoryTracker;
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var inventoryTracker = await _trackerManager.GetOneTrackerAsync((int)id);
			//var inventorytracker = await _context.InventoryTracker.FindAsync(id);

			if (inventoryTracker != null)
			{
				var storages = await _storageManager.GetStoragesAsync(false);
				var storage = storages.Find(s => s.Id == inventoryTracker.StorageId);

				var products = await _productManager.GetProductsAsync(false);
				var product = products.Find(p => p.Id == inventoryTracker.ProductId);


				//var storage = await _context.Storages.FindAsync(inventorytracker.StorageId);

				//var product = await _context.Products.FindAsync(inventorytracker.ProductId);

				if (storage != null && product != null)
				{
					storage.CurrentStock = storage.CurrentStock - inventoryTracker.Quantity;
					storage.Updated = DateTime.Now;
					product.CurrentStock += inventoryTracker.Quantity;
				}

				InventoryTracker = inventoryTracker;
				await _trackerManager.DeleteTrackerAsync(InventoryTracker.Id);
				await _productManager.EditProductAsync(product);
				await _storageManager.EditStorageAsync(storage);

				//_context.InventoryTracker.Remove(InventoryTracker);

				//await _context.SaveChangesAsync();
			}


			return RedirectToPage("./Index");
		}
	}
}
