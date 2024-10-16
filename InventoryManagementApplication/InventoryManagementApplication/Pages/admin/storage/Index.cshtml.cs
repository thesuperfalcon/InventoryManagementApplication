using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagementApplication.Pages.admin.storage
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly StorageManager _storageManager;

        public IndexModel(StorageManager storageManager)
        {
            _storageManager = storageManager;
        }

        public int StorageCount { get; set; }
        public IList<Storage> Storages { get; set; } = default!;

        [BindProperty]
        public bool IsDeletedToggle { get; set; }

        public async Task OnGetAsync(bool isDeletedToggle = false)
        {
            IsDeletedToggle = isDeletedToggle;
            Storages = await LoadStorages(IsDeletedToggle);
            StorageCount = Storages.Count();
        }

        public async Task<IActionResult> OnPostAsync(int buttonId)
        {
            if (buttonId == 1)
            {
                IsDeletedToggle = !IsDeletedToggle;
                return RedirectToPage("./Index", new { isDeletedToggle = IsDeletedToggle });
            }

            return Page();
        }
        private async Task<IList<Storage>> LoadStorages(bool isDeleted)
        {
            var storages = await _storageManager.GetStoragesAsync(isDeleted);

            return storages;
        }
    }
}
