using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Data;
using System.Runtime.CompilerServices;

namespace InventoryManagementApplication.Helpers
{
    public class ProductMovementHelpers
    {
        private readonly ProductManager _productManager;
        private readonly StorageManager _storageManager;
        private readonly TrackerManager _trackerManager;
        private readonly InventoryManagementApplicationContext _context;

        public ProductMovementHelpers(ProductManager productManager, StorageManager storageManager, TrackerManager trackerManager, InventoryManagementApplicationContext context)
        {
            _productManager = productManager;
            _storageManager = storageManager;
            _trackerManager = trackerManager;
            _context = context;
        }
        public async Task<Tuple<bool, string>> MoveProductAsync(int productId, int fromStorageId, int toStorageId, int quantity)
        {
            var trackers = await _trackerManager.GetAllTrackersAsync();

            string message = string.Empty;

            var product = await _productManager.GetProductByIdAsync(productId, false);
            var fromStorage = await _storageManager.GetStorageByIdAsync(fromStorageId, false);
            var toStorage = await _storageManager.GetStorageByIdAsync(toStorageId, false);

            if (fromStorage == null || toStorage == null)
            {
                message = "Kan ej finna lagrerna";
                return new Tuple<bool, string>(false, message);
            }

            var fromStorageTracker = trackers.Where(x => x.StorageId ==  fromStorageId && x.ProductId == productId).FirstOrDefault();
            if (fromStorageTracker == null)
            {
                message = "Kan ej finna lagersaldo från lager";
                return new Tuple<bool, string>(false, message);
            }
            var toStorageTracker = trackers.Where(x => x.StorageId == toStorageId && x.ProductId == productId).FirstOrDefault();
            if (toStorageTracker == null)
            {
                toStorageTracker = new Models.InventoryTracker
                {
                    ProductId = productId,
                    StorageId = toStorageId,
                    Quantity = quantity
                };
                await _trackerManager.CreateTrackerAsync(toStorageTracker);
            }
            else
            {
                toStorageTracker.Quantity += quantity;
            }

            if (quantity > toStorage.MaxCapacity - toStorage.CurrentStock)
            {
                message = "Finns ej den m�ngden produkter i lagret. F�rs�k med en mindre m�ngd.";
                return new Tuple<bool, string>(false, message);
            }
            fromStorageTracker.Quantity -= quantity;

            //ta bort när trackermanagergetbystorageandproductid är fixad
            trackers = await _trackerManager.GetAllTrackersAsync();
            toStorageTracker = trackers.Where(x => x.StorageId == toStorageId && x.ProductId == productId).FirstOrDefault();

            await _trackerManager.EditTrackerAsync(fromStorageTracker);
            await _trackerManager.EditTrackerAsync(toStorageTracker);

            fromStorage.CurrentStock -= quantity;
            toStorage.CurrentStock += quantity;

            await _storageManager.EditStorageAsync(fromStorage);
            await _storageManager.EditStorageAsync(toStorage);

            await _productManager.EditProductAsync(product);

            message = "Förflyttning lyckades";
            return new Tuple<bool, string>(true, message);
        }
    }
}
