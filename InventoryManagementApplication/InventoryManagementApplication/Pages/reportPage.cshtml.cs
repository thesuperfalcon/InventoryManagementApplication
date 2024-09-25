using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages
{
    public class reportPageModel : PageModel
    {
        private readonly StorageManager _storageManager;
        private readonly TrackerManager _trackerManager;
        
        public reportPageModel(TrackerManager trackerManager, StorageManager storageManager)
        {
            _storageManager = storageManager;
            _trackerManager = trackerManager;
        }

        public List<InventoryTracker> InventoryTracker { get; set; } = new List<InventoryTracker>();    
        public List<Storage> Storages { get; set; } = new List<Storage>();

        public async Task OnGetAsync()
        
        {
            Storages = await _storageManager.GetStoragesAsync(false);

            InventoryTracker = await _trackerManager.GetAllTrackersAsync();
        }
    }
}
