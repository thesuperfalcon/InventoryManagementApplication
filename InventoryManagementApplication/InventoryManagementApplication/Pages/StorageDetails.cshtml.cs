using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InventoryManagementApplication.Models;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementApplication.Pages
{
    public class StorageDetailsModel : PageModel
    {
        private readonly InventoryManagementApplicationContext _context;

        public StorageDetailsModel(InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        public Storage Storage { get; set; }
        public List<ActivityLog> ActivityLogs { get; set; }

        public List<Storage> AllStorages { get; set; }
        public int? ProductId { get; set; }
        public List<InventoryTracker> InventoryTrackers { get; set; }

//För sidor
    public int PageNumber { get; set; } = 1; // Nuvarande sida
    public int TotalPages { get; set; }
    public bool HasPreviousPage => PageNumber > 1; // Har föregående sida
    public bool HasNextPage => PageNumber < TotalPages; // Har nästa sida


        public async Task<IActionResult> OnGetAsync(int id, int pageNumber = 1)
    {
        // Hämta lagret
        Storage = await _context.Storages
            .Include(s => s.InventoryTrackers)
            .ThenInclude(it => it.Product)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (Storage == null)
        {
            return NotFound();
        }

        int pageSize = 10;

        var totalProducts = Storage.InventoryTrackers.Count();
        TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
        PageNumber = pageNumber;

        // Hämta endast de produkter som hör till aktuell sida
        InventoryTrackers = Storage.InventoryTrackers
            .Skip((PageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Hämta aktivitetsloggar
        ActivityLogs = await _context.ActivityLog
            .Include(log => log.User)
            .Where(log => log.ItemType == ItemType.Storage && log.TypeId == id)
            .OrderByDescending(log => log.TimeStamp)
            .Take(5)
            .ToListAsync();

        return Page();
    }

        public async Task<IActionResult> OnPostUpdateStorageAsync(int StorageId, string StorageName, int StorageMaxCapacity)
        {
            var storage = await _context.Storages.FindAsync(StorageId);
            if (storage == null) return NotFound();

            storage.Name = StorageName;
            storage.MaxCapacity = StorageMaxCapacity;
            storage.Updated = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = StorageId });
        }

        public async Task<IActionResult> OnPostUpdateStorageCapacityAsync(int StorageId, string StorageName, int StorageMaxCapacity)
        {
            var storage = await _context.Storages.FindAsync(StorageId);
            if (storage == null) return NotFound();

            storage.Name = StorageName;
            storage.MaxCapacity = StorageMaxCapacity;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = StorageId });
        }
    }
}
