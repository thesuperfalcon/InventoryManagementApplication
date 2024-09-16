using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages.admin.product
{
    public class CreateModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public CreateModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        public string StatusMessage { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var products = await _context.Products.ToListAsync();
            if (products.Count < 15)
            {
                Product.CurrentStock = Product.TotalStock;
                _context.Products.Add(Product);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            else
            {
                StatusMessage = "Max antal produkter är 15";
                return Page();
            }
        }
    }
}
