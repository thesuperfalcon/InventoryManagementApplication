using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages.admin.tracker
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
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
        ViewData["StorageId"] = new SelectList(_context.Storages, "Id", "Name");
            return Page();
        }

        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InventoryTracker InventoryTracker { get; set; } = default!;
        public Storage Storage { get; set; } 
        public Product Product { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            int quantity = 0;

			var existingTracker = await _context.InventoryTracker.Where(x => x.ProductId == InventoryTracker.ProductId && x.StorageId == InventoryTracker.StorageId).FirstOrDefaultAsync();
            if(existingTracker == null)
            {
				_context.InventoryTracker.Add(InventoryTracker);
                quantity = (int)InventoryTracker.Quantity;
				await _context.SaveChangesAsync();
			}
            else
            {
                quantity = (int)existingTracker.Quantity;
            }

            Storage = await _context.InventoryTracker
                .Select(x => x.Storage)
                .Where(x => x.Id == InventoryTracker.StorageId)
                .FirstOrDefaultAsync();

            Product = await _context.InventoryTracker
                .Select(x => x.Product)
                .Where(x => x.Id == InventoryTracker.ProductId)
                .FirstOrDefaultAsync();

            //var storageTracking = await _context.InventoryTracker.Where(x => x.StorageId == Storage.Id).ToListAsync();

            //var currentSpace = storageTracking.Sum(x => x.Quantity);

            //if (currentSpace < quantity)
            //{
            //    StatusMessage = $"Finns ej plats i {Storage.Name}. Välj annan lagerplats!";
            //}


            if (quantity > Product.CurrentStock)
            {
                StatusMessage = $"Antalet produkter du vill lägga till finns ej tillgänglig. Antal produkter utan lager: {Product.CurrentStock}";
                if (InventoryTracker != null && InventoryTracker.Id != 0)
                {
                    _context.InventoryTracker.Remove(InventoryTracker);
                    await _context.SaveChangesAsync();
                }
                return RedirectToPage("./Create");
            }
            else if (Storage.CurrentStock < quantity)
            {
                StatusMessage = $"Finns ej plats i {Storage.Name}. Välj annan lagerplats!";
            }

            //InventoryTracker.Product = Product;
            //InventoryTracker.Storage = Storage;
			Storage.CurrentStock += quantity;
            Product.CurrentStock -= quantity;
            Storage.Updated = DateTime.Now;
			await _context.SaveChangesAsync();


			return RedirectToPage("./Index");
        }
    }
}
