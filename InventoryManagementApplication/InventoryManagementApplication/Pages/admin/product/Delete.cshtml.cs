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
        private static Uri BaseAddress = new Uri("https://localhost:44353/");

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
            using (var client = new HttpClient())
            {
            client.BaseAddress = BaseAddress;
                var response = await client.DeleteAsync($"api/Products/{id}");
                //client.BaseAddress = BaseAddress;
                //var json = JsonSerializer.Serialize(Product);

                //Gör det möjligt att skicka innehåll till API
                //StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                //HttpResponseMessage response = await client.PostAsync("api/Products/", httpContent);
            }
            return RedirectToPage("./Index");
            //Product = await _context.Products.FindAsync(id);
            //if (Product == null)
            //{
            //    return NotFound();
            //}


            //InventoryTrackers = await _context.InventoryTracker
            //    .Where(x => x.ProductId == Product.Id)
            //    .ToListAsync();

            //Storages = await _context.Storages
            //    .Where(storage => storage.InventoryTrackers.Any(tracker => InventoryTrackers.Contains(tracker)))
            //    .ToListAsync();

            //foreach (var tracker in InventoryTrackers)
            //{
            //    var storage = Storages.FirstOrDefault(s => s.Id == tracker.StorageId);
            //    if (storage != null)
            //    {
            //        storage.CurrentStock -= tracker.Quantity; 
            //        _context.Storages.Update(storage); 
            //    }
            //}

            _context.Products.Remove(Product);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
