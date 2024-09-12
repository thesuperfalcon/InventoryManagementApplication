using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.Identity.Client.Extensions.Msal;

namespace InventoryManagementApplication.Pages.admin.tracker
{
    public class DeleteModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public DeleteModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventorytracker = await _context.InventoryTracker.FindAsync(id);

			if (inventorytracker != null)
            {
				var storage = await _context.Storages.FindAsync(inventorytracker.StorageId);

                var product = await _context.Products.FindAsync(inventorytracker.ProductId);

                if(storage != null && product != null)
                {
                    storage.CurrentStock = storage.CurrentStock - inventorytracker.Quantity;
                    storage.Updated = DateTime.Now;
                    product.CurrentStock += inventorytracker.Quantity;
                }

				InventoryTracker = inventorytracker;
                _context.InventoryTracker.Remove(InventoryTracker);

                await _context.SaveChangesAsync();
            }


            return RedirectToPage("./Index");
        }
    }
}
