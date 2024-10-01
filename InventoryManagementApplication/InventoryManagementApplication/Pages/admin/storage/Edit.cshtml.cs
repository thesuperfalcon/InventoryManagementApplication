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
		[BindProperty]
        public Storage Storage { get; set; } = default!;

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
			if (!ModelState.IsValid)
			{
				return Page();
			}

			try
			{
				var storageNoEdit = await _storageManager.GetStorageByIdAsync(id, false);
				await _storageManager.EditStorageAsync(Storage);
				await _activityLogManager.LogActivityAsync(Storage, EntityState.Modified, storageNoEdit);
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
