using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using System.Text.Json;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System.Text;
using InventoryManagementApplication.DAL;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementApplication.Pages.admin.storage
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
		private readonly StorageManager _storageManager;
		private readonly LogManager _activityLogManager;

        public EditModel(StorageManager storageManager, LogManager activityLogManager)
        {
			_storageManager = storageManager;
			_activityLogManager = activityLogManager;
        }
		
		[TempData] 
		public string StatusMessage { get; set; }

        [TempData]
        public string StatusMessage1 { get; set; }

        [BindProperty]
        public Storage Storage { get; set; } = default!;
        public int StorageCount { get; set; }

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

            var result = await _storageManager.CheckStorageName(Storage.Name);
            if (result == true)
            {
                StatusMessage = "Lagerplats med samma namn existerar. Skriv in nytt namn";
                return Page();
            }

            if(Storage.IsDeleted == true)
            {
                Storage.IsDeleted = false;
            }

            try
			{
				if(Storage.MaxCapacity < Storage.CurrentStock)
				{
					StatusMessage = $"Går ej att ändra max antal platser lägre än nuvarande produkter: {Storage.CurrentStock}";

                    return RedirectToPage("./Edit", new { id = Storage.Id });
                }
                if ( storageNoChanges.Name != Storage.Name)
                {
                    StatusMessage1 = $"Du har ändrat lagernamn från {storageNoChanges.Name} till: {Storage.Name}";
                }
                if (storageNoChanges.MaxCapacity != Storage.MaxCapacity)
                {
                    StatusMessage1 = $"Du har ändrat max antal platser från: {storageNoChanges.MaxCapacity} till: {Storage.MaxCapacity}" ;
                }
				
				await _storageManager.EditStorageAsync(Storage);

                return RedirectToPage("./Edit", new { id = Storage.Id });

            }
			catch (DbUpdateConcurrencyException)
			{
				if (!await StorageExists(Storage.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

        }

        private async Task<bool> StorageExists(int id)
        {
			var storages = await _storageManager.GetStoragesAsync(false);
			return storages.Any(s => s.Id == id);
        }
    }
}
