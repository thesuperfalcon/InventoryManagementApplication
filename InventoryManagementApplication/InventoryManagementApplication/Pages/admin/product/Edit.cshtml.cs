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

namespace InventoryManagementApplication.Pages.admin.product
{
    public class EditModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public EditModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product =  await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var inventoryTrackers = await _context.InventoryTracker.Where(x => x.ProductId == Product.Id).ToListAsync();

            var productQuantity = inventoryTrackers.Sum(x => x.Quantity);

            if (inventoryTrackers != null && inventoryTrackers.Count > 0)
            {

                if (Product.TotalStock < productQuantity)
                {
                    StatusMessage = $"Går ej att ändra. Currentstock: {Product.CurrentStock} Total: {Product.TotalStock}";
                    return RedirectToPage("./Edit", new { id = Product.Id });
                }
                else
                {
                    //Product.CurrentStock += productQuantity;
                }
            }
            else
            {
                Product.CurrentStock = Product.TotalStock;
            }
            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
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

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
