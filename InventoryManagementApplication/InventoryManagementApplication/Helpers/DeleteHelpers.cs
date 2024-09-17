using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Helpers
{
    public class DeleteHelpers
    {
        private readonly InventoryManagementApplicationContext _context;
        public DeleteHelpers(InventoryManagementApplicationContext context)
        {
            _context = context;
        }
        public async Task RemoveRelateProductDataAsync(Product product)
        {
            var inventoryTrackers = await _context.InventoryTracker
                .Where(x => x.ProductId == product.Id)
                .ToListAsync();

            var relatedStatistics = await _context.Statistics
                .Where(s => s.ProductId == product.Id)
                .ToListAsync();

            foreach (var tracker in inventoryTrackers)
            {
                var storage = await _context.Storages.FindAsync(tracker.StorageId);
                if (storage != null)
                {
                    storage.CurrentStock -= tracker.Quantity;
                    _context.Storages.Update(storage);
                }
                _context.InventoryTracker.Remove(tracker);
            }

            _context.Statistics.RemoveRange(relatedStatistics);

            _context.Products.Remove(product);
        }
    }
}
