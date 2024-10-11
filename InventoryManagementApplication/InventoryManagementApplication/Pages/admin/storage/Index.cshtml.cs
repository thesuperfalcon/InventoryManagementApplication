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
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementApplication.Pages.admin.storage
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly StorageManager _storageManager;
        public int StorageCount { get; set; }
        public IndexModel(StorageManager storageManager)
        {
            _storageManager = storageManager;
        }
		public IList<Storage> Storages { get;set; } = default!;

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
            if(buttonId == 1)
            {
                IsDeletedToggle = !IsDeletedToggle;              
                return RedirectToPage("./Index", new {isDeletedToggle = IsDeletedToggle});
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
