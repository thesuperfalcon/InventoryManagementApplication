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
    public class IndexModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public IndexModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        public IList<InventoryTracker> InventoryTracker { get;set; } = default!;

        public async Task OnGetAsync()
        {
            InventoryTracker = await _context.InventoryTracker
                .Include(i => i.Product)
                .Include(i => i.Storage).ToListAsync();
        }
    }
}
