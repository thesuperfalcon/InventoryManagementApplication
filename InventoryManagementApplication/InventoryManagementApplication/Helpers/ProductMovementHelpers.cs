using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Data;

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
            string message = string.Empty;

            var product = await _productManager.GetProductByIdAsync(productId, false);
            var fromStorage = await _storageManager.GetStorageByIdAsync(fromStorageId, false);
            var toStorage = await _storageManager.GetStorageByIdAsync(toStorageId, false);
            var defaultStorage = await _storageManager.GetDefaultStorageAsync();

            var fromStorageTracker = await _trackerManager.GetTrackerByProductAndStorageAsync(productId, fromStorageId);

            bool success = false;
            if (!success)
            {
                switch ((bool)success)
                {
                    case false when fromStorage == null || toStorage == null:
                        message = "Kan ej finna lagrerna";
                        return new Tuple<bool, string>(false, message);
                    case false when quantity > toStorage.MaxCapacity - toStorage.CurrentStock:
                        message = "Finns ej plats för denna mängd i lagret. Försök med en mindre mängd.";
                        return new Tuple<bool, string>(false, message);
                    case false when fromStorageTracker == null:
                        message = "Kan ej finna lagersaldo från lager";
                        return new Tuple<bool, string>(false, message);
                    case false when fromStorageTracker.Quantity < quantity:
                        message = "Finns ej den mängden produkter i lagret.";
                        return new Tuple<bool, string>(false, message);
                    default:
                        success = true;
                        break;
                }
            }

            var toStorageTracker = await _trackerManager.GetTrackerByProductAndStorageAsync(productId, toStorageId);

            if (toStorageTracker == null)
            {
                toStorageTracker = new Models.InventoryTracker
                {
                    ProductId = productId,
                    StorageId = toStorageId,
                    Quantity = quantity
                };
                await _trackerManager.CreateTrackerAsync(toStorageTracker);

                toStorageTracker = await _trackerManager.GetTrackerByProductAndStorageAsync(productId, toStorageId);
            }
            else
            {
                toStorageTracker.Quantity += quantity;
            }

            fromStorageTracker.Quantity -= quantity;

            await _trackerManager.EditTrackerAsync(fromStorageTracker);
            await _trackerManager.EditTrackerAsync(toStorageTracker);

            fromStorage.CurrentStock -= quantity;
            toStorage.CurrentStock += quantity;

            await _storageManager.EditStorageAsync(fromStorage);
            await _storageManager.EditStorageAsync(toStorage);

            if (defaultStorage.Id == fromStorageId || defaultStorage.Id == toStorageId)
            {
                switch (defaultStorage.Id)
                {
                    case var id when id == fromStorageId:
                        product.CurrentStock -= quantity;
                        break;

                    case var id when id == toStorageId:
                        product.CurrentStock += quantity;
                        break;

                    default:
                        break;
                }
                await _productManager.EditProductAsync(product);
            }

            //message = "Förflyttning lyckades";
            return new Tuple<bool, string>(true, message);
        }
    }
}