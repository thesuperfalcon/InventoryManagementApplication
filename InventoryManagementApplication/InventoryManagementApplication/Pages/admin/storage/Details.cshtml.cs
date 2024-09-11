using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;

namespace InventoryManagementApplication.Pages.admin.storage
{
    public class DetailsModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public DetailsModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        public Storage Storage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storage = await _context.Storages.FirstOrDefaultAsync(m => m.Id == id);
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
