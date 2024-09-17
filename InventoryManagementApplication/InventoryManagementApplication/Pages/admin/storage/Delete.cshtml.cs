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
    public class DeleteModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public DeleteModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public Storage Storage { get; set; } = default!;
        public List<InventoryTracker> Trackers { get; set; }

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storage = await _context.Storages.FindAsync(id);
            var relativeStatistics = await _context.Statistics.Where(x => x.InitialStorageId == id || x.DestinationStorageId == id).ToListAsync();
            Trackers = await _context.InventoryTracker.Where(x => x.StorageId == storage.Id).ToListAsync();
            var sum = Trackers.Sum(x => x.Quantity);
            if(sum > 0)
            {
                StatusMessage = "Går ej att ta bort. Flytta produkterna innann du tar bort lagret!";

                return RedirectToPage("./Delete", new {id = storage.Id});
            }

            if (storage != null)
            {
                Storage = storage;
                _context.RemoveRange(relativeStatistics);
                _context.Storages.Remove(Storage);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
