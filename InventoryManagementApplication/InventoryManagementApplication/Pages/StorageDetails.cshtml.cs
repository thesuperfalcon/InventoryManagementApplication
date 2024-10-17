using InventoryManagementApplication.Models;
using InventoryManagementApplication.DAL;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementApplication.Pages
{
    public class StorageDetailsModel : PageModel
    {
        private readonly StorageManager _storageManager;
        private readonly LogManager _logManager;
        private readonly UserManager _userManager;
        private readonly TrackerManager _trackerManager; 

        public StorageDetailsModel(LogManager logManager, UserManager userManager, StorageManager storageManager, TrackerManager trackerManager)
        {
            _storageManager = storageManager;
            _logManager = logManager;
            _userManager = userManager;
            _storageManager = storageManager;
            _trackerManager = trackerManager;
        }

        public Storage Storage { get; set; }
        public List<Log> ActivityLogs { get; set; } = new List<Log>();
        public List<string> UserFullName { get; set; } = new List<string>();
        public List<string> UserEmployeeNumbers { get; set; } = new List<string>();
        public List<InventoryTracker> InventoryTrackers { get; set; } = new List<InventoryTracker> { };

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Storage = await _storageManager.GetStorageByIdAsync(id, null);


			if (Storage == null)
			{
				return NotFound();
			}

			InventoryTrackers = await _trackerManager.GetTrackerByProductOrStorageAsync(0, id);

            ActivityLogs = await _logManager.GetLogByForEntityAsync("Storage", id);
           
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStorageAsync(int StorageId, string StorageName)
        {
            // Hämta lagret via API:et
            var storage = await _storageManager.GetStorageByIdAsync(StorageId, null);
            if (storage == null)
            {
                return NotFound();
            }

            var oldStorageName = storage.Name;
            storage.Name = StorageName;
            storage.Updated = DateTime.Now;

            // Spara ändringarna via API:et
            await _storageManager.EditStorageAsync(storage);


            var allLogs = await _logManager.GetAllLogsAsync();
            var logsToUpdate = allLogs
                .Where(log => log.EntityType.Contains("Storage") && log.EntityName == oldStorageName)
                .ToList();

            foreach (var log in logsToUpdate)
            {
                log.EntityName = StorageName; // Uppdatera loggens namn
                await _logManager.EditLogAsync(log);
            }

            return RedirectToPage(new { id = StorageId });
        }
    }
}
