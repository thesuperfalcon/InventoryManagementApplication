using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages.admin.product
{
	public class DeleteModel : PageModel
	{
		private readonly ProductManager _productManager;
		private readonly StorageManager _storageManager;
		private readonly TrackerManager _trackerManager;

		public DeleteModel(ProductManager productManager,
			StorageManager storageManager, TrackerManager trackerManager)
		{
			_productManager = productManager;
			_storageManager = storageManager;
			_trackerManager = trackerManager;
		}

		[BindProperty]
		public Product Product { get; set; } = default!;
		public List<InventoryTracker> InventoryTrackers { get; set; } = new List<InventoryTracker>();
		public List<Storage> Storages { get; set; } = new List<Storage>();

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Product = await _productManager.GetProductByIdAsync(id, null);

			if (Product == null)
			{
				return NotFound();
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{

			if (id == null)
			{
				return NotFound();
			}
			var trackers = await _trackerManager.GetAllTrackersAsync();
			InventoryTrackers = trackers.Where(x => x.ProductId == id).ToList();

			var storages = await _storageManager.GetStoragesAsync(false);
			

			foreach (var tracker in InventoryTrackers)
			{
				Models.Storage storage = storages.FirstOrDefault(s => s.Id == tracker.StorageId);
				if (storage != null)
				{
					storage.CurrentStock -= tracker.Quantity;
					_storageManager.EditStorageAsync(storage);
					_trackerManager.DeleteTrackerAsync(tracker.Id);
				}
			}
			_productManager.DeleteProductAsync(id);

			return RedirectToPage("./Index");
		}
	}
}
