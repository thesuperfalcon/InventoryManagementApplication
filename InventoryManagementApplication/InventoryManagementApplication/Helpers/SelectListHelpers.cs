using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Models;
using InventoryManagementApplication.Data;
using Microsoft.Identity.Client.Extensions.Msal;

namespace InventoryManagementApplication.Helpers
{
    public class SelectListHelpers
    {
        private readonly InventoryManagementApplicationContext _context;
        public SelectListHelpers(InventoryManagementApplicationContext context)
        {
           _context = context;
        }

        public async Task<SelectList> GenerateProductSelectListAsync()
        {
            var products = await _context.Products.ToListAsync();

            var productItems = products.Select(x => new
            {
                Value = x.Id,
                Text = $"{x.Name} (Antal utan lager: {x.CurrentStock})"
            });

            return new SelectList(productItems, "Value", "Text");
        }

        public async Task<SelectList> GenerateStorageSelectListAsync(int? storageId)
        {
            var storages = await _context.Storages.ToListAsync();

            var storageItems = storages
                .Select(x => new
                {
                    Value = x.Id,
                    Text = $"{x.Name} (Lediga platser: {x.MaxCapacity - x.CurrentStock})"
                })
                .ToList();

            if (storageId.HasValue && storageId.Value != 0)
            {
                storageItems = storageItems
                    .Where(x => x.Value != storageId.Value)
                    .ToList();
            }

            return new SelectList(storageItems, "Value", "Text");
        }

    }
}
