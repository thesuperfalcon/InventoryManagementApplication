using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using InventoryManagementApplication.Pages.admin.storage;
using System.Threading.Tasks;

namespace InventoryManagementApplication.Helpers
{
    public class ProductMovementHelpers
    {
        private readonly ProductManager _productManager;
        private readonly StorageManager _storageManager;
        private readonly TrackerManager _trackerManager;
        private readonly StatisticManager _statisticManager;
        public ProductMovementHelpers(ProductManager productManager, StorageManager storageManager, TrackerManager trackerManager, StatisticManager statisticManager)
        {
            _productManager = productManager;
            _storageManager = storageManager;
            _statisticManager = statisticManager;
            _trackerManager = trackerManager;
        }

        public async Task<OperationResult> MoveProductAsync(int productId, int fromStorageId, int toStorageId, int quantity)
        {
            var product = await _productManager.GetProductByIdAsync(productId, false);
            var fromStorage = await _storageManager.GetStorageByIdAsync(fromStorageId, false);
            var toStorage = await _storageManager.GetStorageByIdAsync(toStorageId, false);
            var defaultStorage = await _storageManager.GetDefaultStorageAsync();

            var fromStorageTracker = await _trackerManager.GetTrackerByProductAndStorageAsync(productId, fromStorageId);
            if (!await ValidateMoveAsync(productId, fromStorage, toStorage, fromStorageTracker, quantity))
            {
                return new OperationResult(false, "Validation failed.");
            }

            var toStorageTracker = await _trackerManager.GetTrackerByProductAndStorageAsync(productId, toStorageId) ??
                await CreateToStorageTrackerAsync(productId, toStorageId, quantity);

            UpdateTrackers(fromStorageTracker, toStorageTracker, quantity);
            await UpdateStoragesAsync(fromStorage, toStorage, quantity);
            await UpdateProductStockAsync(defaultStorage, fromStorageId, toStorageId, product, quantity);
            await CreateMovementStatisticAsync(productId, fromStorageId, toStorageId, quantity);
            return new OperationResult(true, string.Empty);
        }

        private async Task<bool> ValidateMoveAsync(int productId, Storage fromStorage, Storage toStorage, InventoryTracker fromStorageTracker, int quantity)
        {
            if (fromStorage == null || toStorage == null) return false;
            if (quantity > toStorage.MaxCapacity - toStorage.CurrentStock) return false;
            if (fromStorageTracker == null || fromStorageTracker.Quantity < quantity) return false;

            return true;
        }

        private async Task<InventoryTracker> CreateToStorageTrackerAsync(int productId, int toStorageId, int quantity)
        {
            var newTracker = new InventoryTracker
            {
                ProductId = productId,
                StorageId = toStorageId,
                Quantity = quantity
            };
            await _trackerManager.CreateTrackerAsync(newTracker);
            return newTracker;
        }

        private void UpdateTrackers(InventoryTracker fromTracker, InventoryTracker toTracker, int quantity)
        {
            fromTracker.Quantity -= quantity;
            toTracker.Quantity += quantity;
        }

        private async Task UpdateStoragesAsync(Storage fromStorage, Storage toStorage, int quantity)
        {
            fromStorage.CurrentStock -= quantity;
            toStorage.CurrentStock += quantity;

            await _storageManager.EditStorageAsync(fromStorage);
            await _storageManager.EditStorageAsync(toStorage);
        }

        private async Task UpdateProductStockAsync(Storage defaultStorage, int fromStorageId, int toStorageId, Product product, int quantity)
        {
            if (defaultStorage.Id == fromStorageId)
            {
                product.CurrentStock -= quantity;
            }
            else if (defaultStorage.Id == toStorageId)
            {
                product.CurrentStock += quantity;
            }
            await _productManager.EditProductAsync(product);
        }
        private async Task CreateMovementStatisticAsync(int productId, int fromStorageId, int toStorageId, int quantity)
        {

            await _statisticManager.GetValueFromStatisticAsync(fromStorageId, toStorageId, productId, quantity, null);
        }
    }

}

public class OperationResult
{
    public bool Success { get; }
    public string Message { get; }

    public OperationResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}
