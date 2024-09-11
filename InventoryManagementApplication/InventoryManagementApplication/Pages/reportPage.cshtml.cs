using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages
{
    public class reportPageModel : PageModel
    {
        private readonly InventoryManagementApplicationContext _context;
        
        public reportPageModel(InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        public List<InventoryTracker> InventoryTracker { get; set; } = new List<InventoryTracker>();    
        public List<Storage> Storages { get; set; } = new List<Storage>();

        public async Task OnGetAsync()
        {

            Storages = await _context.Storages
                .Include(x => x.InventoryTrackers)
                .ThenInclude(x => x.Product)
                .OrderBy(x => x.Id)
                .ToListAsync();

            InventoryTracker = await _context.InventoryTracker
                .Include(x => x.Storage)
                .Include(x => x.Product)
                .OrderBy(x => x.StorageId)
                .ToListAsync();
        }
    }
}
