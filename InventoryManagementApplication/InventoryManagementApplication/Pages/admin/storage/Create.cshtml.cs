using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages.admin.storage
{
	[Authorize(Roles = "Admin")]
	public class CreateModel : PageModel
	{
        
        private readonly StorageManager _storageManager;
		private readonly LogManager _logManager;

		public CreateModel(StorageManager storageManager, LogManager logManager)
		{
			_storageManager = storageManager;
            _logManager = logManager;
		}

		[BindProperty]
		public Storage Storage { get; set; } = default!;
		public string StatusMessage { get; set; }
        public string StatusMessage1 { get; set; }
        public int StorageCount { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			var storages = await _storageManager.GetStoragesAsync(false);
			StorageCount = storages.Count();

            if (StorageCount >= 7)
            {
                StatusMessage = $"Det finns redan 6 stycken lagerplatser vilket är max! Är du säker på att du vill lägga till fler?";
            }
            return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var existingStorages = await _storageManager.GetStoragesAsync(false);

			var existingStorageName = existingStorages.FirstOrDefault(x => x.Name == Storage.Name);
			StorageCount = existingStorages.Count();

			if(existingStorageName != null)
			{
				StatusMessage = "Ett lager med detta namn finns redan. Välj ett annat namn";
				return Page();
			}

			await _storageManager.CreateStorageAsync(Storage);
			StatusMessage1 = $"Du har skapat {Storage.Name} med {Storage.MaxCapacity} platser!";

			await _logManager.LogActivityAsync(Storage, EntityState.Added);

			return Page();
		}
	}
}
