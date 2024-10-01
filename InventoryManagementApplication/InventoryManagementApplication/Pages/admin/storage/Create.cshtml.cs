using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages.admin.storage
{
	public class CreateModel : PageModel
	{
		private readonly StorageManager _storageManager;
		private readonly LogManager _activityLogManager;
		public CreateModel(StorageManager storageManager, LogManager activityLogManager)
		{
			_storageManager = storageManager;
			_activityLogManager = activityLogManager;
		}

		[BindProperty]
		public Storage Storage { get; set; } = default!;
		public string StatusMessage { get; set; }

		public IActionResult OnGet()
		{
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			await _storageManager.CreateStorageAsync(Storage);
			var createdStorage = await _storageManager.GetStorageByNameAsync(Storage.Name);
			await _activityLogManager.LogActivityAsync(createdStorage, EntityState.Added);

			return RedirectToPage("./Index");
		}
	}
}
