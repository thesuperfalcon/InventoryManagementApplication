using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages.admin.storage
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
	{
		private readonly StorageManager _storageManager;
		private readonly TrackerManager _trackerManager;
		private readonly LogManager _activityLogManager;

		public DeleteModel(StorageManager storageManager, TrackerManager trackerManager, LogManager activityLogManager)
		{
			_storageManager = storageManager;
			_trackerManager = trackerManager;
			_activityLogManager = activityLogManager;
		}

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
			var storage = await _storageManager.GetStorageByIdAsync(id, null);
			Trackers = await _trackerManager.GetAllTrackersAsync();
			Trackers = Trackers.Where(x => x.StorageId == storage.Id).ToList();

			var sum = Trackers.Sum(x => x.Quantity);
			if (sum > 0)
			{
				return RedirectToPage("./Delete", new { id = storage.Id });
			}
			if (storage != null)
			{
				await _activityLogManager.LogActivityAsync(Storage, EntityState.Deleted);
				await _storageManager.DeleteStorageAsync(id);
			}

			return RedirectToPage("./Index");

		}
	}
}
