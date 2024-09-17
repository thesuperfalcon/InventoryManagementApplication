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
using System.Net.WebSockets;

namespace InventoryManagementApplication.Pages.admin.tracker
{
    public class CreateModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public CreateModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		[TempData]
        public string StatusMessage { get; set; }
        public SelectList StorageSelectList { get; set; }
        public SelectList ProductSelectList { get; set; }
        [BindProperty]
        public InventoryTracker InventoryTracker { get; set; } = default!;
        public Storage Storage { get; set; }
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //Allt returnerar null, får inte upp några alternativ på Create Tracker
            var storages = await _context.Storages.ToListAsync();
            var products = await _context.Products.ToListAsync();

            var productItems = products.Select(x => new
            {
                Value = x.Id,
                Text = $"{x.Name} (Antal utan lager: {x.CurrentStock})"
            });

            var storageItems = storages.Select(x => new
            {
                Value = x.Id,
                Text = $"{x.Name} (Lediga platser: {x.MaxCapacity - x.CurrentStock})"
            });

            StorageSelectList = new SelectList(storageItems, "Value", "Text");
            ProductSelectList = new SelectList(productItems, "Value", "Text");

            return Page();
        }


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
                quantity = (int)InventoryTracker.Quantity;
            }
            Storage = await _context.InventoryTracker
                .Select(x => x.Storage)
                .Where(x => x.Id == InventoryTracker.StorageId)
                .FirstOrDefaultAsync();

            Product = await _context.InventoryTracker
                .Select(x => x.Product)
                .Where(x => x.Id == InventoryTracker.ProductId)
                .FirstOrDefaultAsync();

            var currentSpace = Storage.MaxCapacity - Storage.CurrentStock;


            if (currentSpace < quantity)
            {
                StatusMessage = $"Finns ej plats i {Storage.Name}. Välj annan lagerplats!";
                if(existingTracker == null)
                {
                    _context.Remove(InventoryTracker);
                    await _context.SaveChangesAsync();  
                }
                return RedirectToPage("./Create");

            }


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

            if(existingTracker != null)
            {
                existingTracker.Quantity += InventoryTracker.Quantity;
            }
			Storage.CurrentStock += quantity;
            Product.CurrentStock -= quantity;
            Storage.Updated = DateTime.Now;
			await _context.SaveChangesAsync();


			return RedirectToPage("./Index");
        }
    }
}
