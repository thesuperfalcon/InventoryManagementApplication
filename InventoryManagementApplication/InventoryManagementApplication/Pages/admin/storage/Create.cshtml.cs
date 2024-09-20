using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagementApplication.Pages.admin.storage
{
	public class CreateModel : PageModel
	{
		private readonly StorageManager _storageManager;

		public CreateModel(StorageManager storageManager)
		{
			_storageManager = storageManager;
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

			return RedirectToPage("./Index");
		}
	}
}
