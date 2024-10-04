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
    public class IndexModel : PageModel
    {
        private readonly StorageManager _storageManager;
        public int StorageCount { get; set; }
        public IndexModel(StorageManager storageManager)
        {
            _storageManager = storageManager;
        }
		public IList<Storage> Storages { get;set; } = default!;

        public async Task OnGetAsync()
        {
            //ändra till false
			Storages = await _storageManager.GetStoragesAsync(null);
            var storages = await _storageManager.GetStoragesAsync(false);
            StorageCount = storages.Count();
		}
    }
}
