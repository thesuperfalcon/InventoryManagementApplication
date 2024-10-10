using InventoryManagementApplication.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagementApplication.Pages.admin.product
{
    [Authorize]
    public class SellProductModel : PageModel
    {
        private readonly ProductManager _productManager;
        private readonly TrackerManager _trackerManager;
        private readonly StorageManager _storageManager;
        public SellProductModel(ProductManager productManager, TrackerManager trackerManager, StorageManager storageManager)
        {
            _productManager = productManager;
            _trackerManager = trackerManager;
            _storageManager = storageManager;
        }

        public Models.Storage Storage { get; set; }

        public Models.Product Product { get; set; }
        public List<Models.InventoryTracker> InventoryTrackers { get; set; }
        public Models.InventoryTracker Tracker { get; set; }
        public List<int?> CurrentProductAmountList { get; set; }

        [BindProperty]
        public int StorageId { get; set; }

        [BindProperty]
        public int ProductId { get; set; }

        public int Index { get; set; }

        [BindProperty]
        public int SellAmount { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public async Task OnGetAsync(int id, int storageId, int index)
        {
            ProductId = id;
            StorageId = storageId;
            Index = index;
            if(storageId != 0)
            {
                Storage = await _storageManager.GetStorageByIdAsync(StorageId, null);
            }
           
            Product = await _productManager.GetProductByIdAsync(id, null);
            var getTrackers = await _trackerManager.GetAllTrackersAsync();
            InventoryTrackers = getTrackers.Where(x => x.ProductId == id).ToList();
            CurrentProductAmountList = InventoryTrackers.Where(t => t.ProductId == id).Select(x => x.Quantity).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Product = await _productManager.GetProductByIdAsync(ProductId, null);
            Storage = await _storageManager.GetStorageByIdAsync(StorageId, null);
            var getTrackers = await _trackerManager.GetAllTrackersAsync();
            InventoryTrackers = getTrackers.Where(x => x.ProductId == Product.Id).ToList();
            int? currentProductAmount = InventoryTrackers.Where(t => t.StorageId == Storage.Id && t.ProductId == Product.Id).Select(x => x.Quantity).FirstOrDefault();
            Tracker = InventoryTrackers.Where(t => t.StorageId == Storage.Id && t.ProductId == Product.Id).SingleOrDefault();

            if (SellAmount > currentProductAmount)
            {
                StatusMessage = "Du kan inte sälja mer produkter än vad som finns i lagret!";
                return RedirectToPage("./SellProduct", new { index = Index, id = Product.Id, storageId = Storage.Id });
            }
            else if(SellAmount <= 0)
            {
                StatusMessage = "Du kan inte sälje färre än 1 produkt!";
                return RedirectToPage("./SellProduct", new { index = Index, id = Product.Id, storageId = Storage.Id });
            }
            else
            {
                Tracker.Quantity -= SellAmount;
                Storage.CurrentStock -= SellAmount;
                Product.TotalStock -= SellAmount;
                if(Storage.Id == 1)
                {
                    Product.CurrentStock -= SellAmount;
                }
                
                await _productManager.EditProductAsync(Product);
                await _trackerManager.EditTrackerAsync(Tracker);
                await _storageManager.EditStorageAsync(Storage);
                StatusMessage = "Nu har du sålt " + SellAmount + " antal " + Product.Name;
                return RedirectToPage("./SellProduct", new { storageId = 0, id = Product.Id });
            }

            
        }
    }
}
