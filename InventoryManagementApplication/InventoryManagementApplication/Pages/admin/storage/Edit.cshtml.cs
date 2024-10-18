using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages.admin.storage
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly StorageManager _storageManager;
        private readonly LogManager _activityLogManager;

        public EditModel(StorageManager storageManager, LogManager activityLogManager)
        {
            _storageManager = storageManager;
            _activityLogManager = activityLogManager;
        }

        [BindProperty]
        public Storage Storage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storage = await _storageManager.GetStorageByIdAsync(id, null);

            if (storage == null)
            {
                return NotFound();
            }

            Storage = storage;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int id = Storage.Id;
            var storageNoChanges = await _storageManager.GetStorageByIdAsync(id, null);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kontrollera om det nya namnet redan finns innan andra ändringar behandlas
            if (storageNoChanges.Name != Storage.Name)
            {
                var isDuplicateName = await _storageManager.CheckStorageName(Storage.Name);
                if (isDuplicateName == true)
                {
                    TempData["StatusMessageError"] = "Lagerplats med samma namn existerar. Skriv in nytt namn";
                    return Page();
                }
            }

            // Kontrollera om MaxCapacity är mindre än CurrentStock
            if (Storage.MaxCapacity < Storage.CurrentStock)
            {
                TempData["StatusMessageError"] = $"Går ej att ändra max antal platser lägre än nuvarande produkter: {Storage.CurrentStock}";

                return RedirectToPage("./Edit", new { id = Storage.Id });
            }

            // Om det inte är ett duplicerat namn, behandla kapacitetsändringar
            if (storageNoChanges.MaxCapacity != Storage.MaxCapacity)
            {
                TempData["StatusMessageSuccess"] = $"Du har ändrat max antal platser från: {storageNoChanges.MaxCapacity} till {Storage.MaxCapacity}";
            }

            // Om namnet har ändrats och det inte finns några dubbletter
            if (storageNoChanges.Name != Storage.Name)
            {
                TempData["StatusMessageSuccess"] = $"Du har ändrat lagernamn från {storageNoChanges.Name} till {Storage.Name}";
            }


            if (Storage.IsDeleted == true)
            {
                var isDuplicateName = await _storageManager.CheckStorageName(Storage.Name);
                if (isDuplicateName == true)
                {
                    TempData["StatusMessageError"] = "Lagerplats med samma namn existerar. Skriv in nytt namn";
                    return Page();
                }

                Storage.IsDeleted = false;
            }

            await _storageManager.EditStorageAsync(Storage);
            await _activityLogManager.LogActivityAsync(Storage, EntityState.Modified, storageNoChanges);

            return RedirectToPage("./Edit", new { id = Storage.Id });
        }

        private async Task<bool> StorageExists(int id)
        {
            var storages = await _storageManager.GetStoragesAsync(false);
            return storages.Any(s => s.Id == id);
        }
    }
}
