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
    public class IndexModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public IndexModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        public IList<Storage> Storage { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Storage = await _context.Storages.ToListAsync();
        }
    }
}
