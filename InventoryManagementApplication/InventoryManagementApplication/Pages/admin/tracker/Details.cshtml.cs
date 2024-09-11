using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;

namespace InventoryManagementApplication.Pages.admin.tracker
{
    public class DetailsModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public DetailsModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        public InventoryTracker InventoryTracker { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventorytracker = await _context.InventoryTracker.FirstOrDefaultAsync(m => m.Id == id);
            if (inventorytracker == null)
            {
                return NotFound();
            }
            else
            {
                InventoryTracker = inventorytracker;
            }
            return Page();
        }
    }
}
