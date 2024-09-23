using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using System.Text.Json;
using InventoryManagementApplication.DAL;

namespace InventoryManagementApplication.Pages.admin.storage
{
    public class DetailsModel : PageModel
    {
        private readonly StorageManager _storaManager;

        public DetailsModel(StorageManager storageManager)
        {
            _storaManager = storageManager;
        }
        public Storage Storage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var storage = await _storaManager.GetOneStorageAsync(id);

			if (storage == null)
			{
				return NotFound();
			}
			else
			{
				Storage = storage;
			}

			return Page();		
        }
    }
}
