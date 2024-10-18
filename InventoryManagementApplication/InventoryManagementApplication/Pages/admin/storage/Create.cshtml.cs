using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages.admin.storage
{
	[Authorize]
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

        public int StorageCount { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			var storages = await _storageManager.GetStoragesAsync(false);
			StorageCount = storages.Count();

            return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var result = await _storageManager.CheckStorageName(Storage.Name);
			if(result == true)
			{
                TempData["StatusMessageError"] = "Lagerplats med samma namn existerar. Skriv in nytt namn";
				return Page();
			}
			
			await _storageManager.CreateStorageAsync(Storage);
				TempData["StatusMessageSuccess"] = $"Du har skapat {Storage.Name} med {Storage.MaxCapacity} platser!";

			return Page();
		}
	}
}
