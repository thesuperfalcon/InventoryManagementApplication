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

namespace InventoryManagementApplication.Pages.admin.tracker
{
    public class EditModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public EditModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
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

            var inventorytracker =  await _context.InventoryTracker.FirstOrDefaultAsync(m => m.Id == id);
            if (inventorytracker == null)
            {
                return NotFound();
            }
            InventoryTracker = inventorytracker;
           ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
           ViewData["StorageId"] = new SelectList(_context.Storages, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(InventoryTracker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryTrackerExists(InventoryTracker.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool InventoryTrackerExists(int id)
        {
            return _context.InventoryTracker.Any(e => e.Id == id);
        }
    }
}
