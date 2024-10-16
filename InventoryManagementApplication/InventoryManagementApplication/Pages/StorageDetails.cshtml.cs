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

        public StorageDetailsModel(StorageManager storageManager, LogManager logManager, UserManager userManager, TrackerManager trackerManager)
        {
            _storageManager = storageManager;
            _logManager = logManager;
            _userManager = userManager;
            _trackerManager = trackerManager;
        }

        public Storage Storage { get; set; }
        public List<Log> ActivityLogs { get; set; } = new List<Log>();
        public List<string> UserFullName { get; set; } = new List<string>();
        public List<string> UserEmployeeNumbers { get; set; } = new List<string>();
        public ICollection<InventoryTracker> InventoryTrackers { get; set; } = new List<InventoryTracker>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Hämta lagret via API:et
            Storage = await _storageManager.GetStorageByIdAsync(id, null);


            if (Storage == null)
            {
                return NotFound();
            }

            var allTrackers = await _trackerManager.GetAllTrackersAsync();

            InventoryTrackers = allTrackers.Where(it => it.StorageId == id).ToList();

            var allLogs = await _logManager.GetAllLogsAsync();
            ActivityLogs = allLogs
                .Where(log => log.EntityType.Contains("Storage") && log.Id == Storage.Id)
                .ToList();

            var users = await _userManager.GetAllUsersAsync(false);
            var userDictionary = users.ToDictionary(
                u => u.Id,
                u => new { FullName = $"{u.LastName} {u.FirstName}", EmployeeNumber = u.EmployeeNumber });

            foreach (var log in ActivityLogs)
            {
                if (userDictionary.TryGetValue(log.UserId, out var userInfo))
                {
                    UserFullName.Add(userInfo.FullName);
                    UserEmployeeNumbers.Add(userInfo.EmployeeNumber);
                }
                else
                {
                    UserFullName.Add("Unknown User");
                    UserEmployeeNumbers.Add("N/A");
                }
            }

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
