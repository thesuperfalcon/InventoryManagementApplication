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

namespace InventoryManagementApplication.Pages.admin.storage
{
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

			var storage = await _storageManager.GetStorageByIdAsync(id, false);

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
			string name = Storage.Name;
            var storageNoChanges = await _storageManager.GetStorageByIdAsync(id, false);

            if (!ModelState.IsValid)
			{
				return Page();
			}
			var storages = await _storageManager.GetStoragesAsync(false);	

            if (storages.Any(x => x.Id != id && x.Name.Equals(Storage.Name, StringComparison.OrdinalIgnoreCase)))
            {
                StatusMessage = "Ett lager med detta namn finns redan. Välj ett annat namn";
                return Page();
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
				await _activityLogManager.LogActivityAsync(Storage, EntityState.Modified, storageNoChanges);

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
