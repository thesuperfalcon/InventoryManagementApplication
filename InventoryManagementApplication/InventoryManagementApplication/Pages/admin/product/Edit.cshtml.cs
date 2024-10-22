using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages.admin.product
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ProductManager _productManager;
        private readonly TrackerManager _trackerManager;
        private readonly StorageManager _storageManager;

        public EditModel(ProductManager productManager, TrackerManager trackerManager, StorageManager storageManager)
        {
            _productManager = productManager;
            _trackerManager = trackerManager;
            _storageManager = storageManager;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        public Product PreviousProduct { get; set; } = default!;
        public List<InventoryTracker> InventoryTrackers { get; set; }
        public int? SellAmount { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productManager.GetProductByIdAsync(id, null);

            if (product == null)
            {
                return NotFound();
            }

            Product = product;
            SellAmount = 0;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int id = Product.Id;
            var productNoChanges = await _productManager.GetProductByIdAsync(id, null);
            bool recreatedProduct = false;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (productNoChanges.Name != Product.Name)
            {
                var existingProductName = await _productManager.CheckProductName(Product.Name);
                if (existingProductName == true)
                {
                    TempData["StatusMessageError"] = "Produkt finns med samma namn. Skriv in ett nytt namn";
                    return Page();
                }
            }

            if (Product.IsDeleted == true)
            {
                recreatedProduct = true;

                var existingProductName = await _productManager.CheckProductName(Product.Name);
                if (existingProductName == true)
                {
                    TempData["StatusMessageError"] = "Produkt finns med samma namn. Skriv in ett nytt namn";
                    return Page();
                }
                Product.IsDeleted = false;
            }

            PreviousProduct = await _productManager.GetProductByIdAsync(Product.Id, null);
            var defaultStorage = await _storageManager.GetDefaultStorageAsync();
            var tracker = await _trackerManager.GetTrackerByProductAndStorageAsync(Product.Id, defaultStorage.Id);

            bool isNewTracker = false;
            if (tracker == null)
            {
                tracker = new InventoryTracker
                {
                    ProductId = Product.Id,
                    StorageId = defaultStorage.Id,
                    Quantity = Product.TotalStock
                };

                await _trackerManager.CreateTrackerAsync(tracker);
                isNewTracker = true;
            }

            if (!isNewTracker)
            {
                InventoryTrackers = await _trackerManager.GetTrackerByProductOrStorageAsync(Product.Id, 0);

                if (InventoryTrackers != null && InventoryTrackers.Count > 0)
                {
                    var productQuantity = InventoryTrackers.Sum(x => x.Quantity);

                    if (Product.TotalStock < productQuantity)
                    {
                        TempData["StatusMessageError"] = $"Det går ej att fylla med färre antal.";
                        return RedirectToPage("./Edit", new { id = Product.Id });
                    }
                    else
                    {
                        var input = Product.TotalStock - productQuantity;
                        Product.CurrentStock += input;
                        defaultStorage.CurrentStock += input;
                        tracker.Quantity += input;
                    }
                }
            }
            else
            {
                Product.CurrentStock = Product.TotalStock;
                defaultStorage.CurrentStock += Product.TotalStock;
            }

            await _productManager.EditProductAsync(Product);
            await _storageManager.EditStorageAsync(defaultStorage);

            if (!isNewTracker)
            {
                await _trackerManager.EditTrackerAsync(tracker);
            }

            string statusMessage = recreatedProduct == true ? "Produkten har återskapats!" : "Produkt har ändrats!";
            TempData["StatusMessageSuccess"] = statusMessage;

            return RedirectToPage("./Edit", new { id = Product.Id });
        }
    }
}
