using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;

namespace InventoryManagementApplication.Pages.admin.product
{
    public class DeleteModel : PageModel
    {
        private readonly InventoryManagementApplicationContext _context;

        public DeleteModel(InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        public List<InventoryTracker> InventoryTrackers { get; set; } = new List<InventoryTracker>();
        public List<Storage> Storages { get; set; } = new List<Storage>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products.FindAsync(id);
            if (Product == null)
            {
                return NotFound();
            }

            InventoryTrackers = await _context.InventoryTracker
                .Where(x => x.ProductId == Product.Id)
                .ToListAsync();

            var relatedStatistics = await _context.Statistics
                .Where(s => s.ProductId == Product.Id)
                .ToListAsync();

            foreach (var tracker in InventoryTrackers)
            {
                var storage = await _context.Storages.FindAsync(tracker.StorageId);
                if (storage != null)
                {
                    storage.CurrentStock -= tracker.Quantity; 
                    _context.Storages.Update(storage);
                }
                _context.InventoryTracker.Remove(tracker); 
            }

            _context.Statistics.RemoveRange(relatedStatistics);

            _context.Products.Remove(Product);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}