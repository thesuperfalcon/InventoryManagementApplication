using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using InventoryManagementApplication.Helpers;

namespace InventoryManagementApplication.Pages.admin.product
{
    public class DeleteModel : PageModel
    {
        private readonly InventoryManagementApplicationContext _context;
        private readonly DeleteHelpers _deleteHelpers;

        public DeleteModel(InventoryManagementApplicationContext context, DeleteHelpers deleteHelpers)
        {
            _context = context;
            _deleteHelpers = deleteHelpers;
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

            await _deleteHelpers.RemoveRelateProductDataAsync(Product);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}