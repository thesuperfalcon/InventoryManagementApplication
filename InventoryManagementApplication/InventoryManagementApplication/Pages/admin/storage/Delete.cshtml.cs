using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagementApplication.Pages.admin.storage
{
	public class DeleteModel : PageModel
	{
		private readonly StorageManager _storageManager;
		private readonly TrackerManager _trackerManager;

		public DeleteModel(StorageManager storageManager, TrackerManager trackerManager)
		{
			_storageManager = storageManager;
			_trackerManager = trackerManager;
		}

		[TempData]
		public string StatusMessage { get; set; }
		[BindProperty]
		public Storage Storage { get; set; } = default!;
		public List<InventoryTracker> Trackers { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var storage = await _storageManager.GetStorageByIdAsync(id, false);

			if (storage == null)
			{
				return NotFound();
			}
			else
			{
				Storage = storage;
			}
			return Page();

		}
		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var storage = await _storageManager.GetStorageByIdAsync(id, false);
			Trackers = await _trackerManager.GetAllTrackersAsync();
			Trackers = Trackers.Where(x => x.StorageId == storage.Id).ToList();

			var sum = Trackers.Sum(x => x.Quantity);
			if (sum > 0)
			{
				StatusMessage = "Går ej att ta bort. Flytta produkterna innann du tar bort lagret!";

				return RedirectToPage("./Delete", new { id = storage.Id });
			}
			if (storage != null)
			{
				await _storageManager.DeleteStorageAsync(id);
			}

			return RedirectToPage("./Index");

		}
	}
}
