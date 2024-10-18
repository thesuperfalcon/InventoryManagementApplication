using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Helpers;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementApplication.Pages.admin.tracker
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ProductManager _productManager;
        private readonly StorageManager _storageManager;
        private readonly SelectListHelpers _selectListHelpers;
        private readonly ProductMovementHelpers _productMovementHelpers;
        private readonly StatisticManager _statisticManager;
        private readonly UserManager<InventoryManagementUser> _userManager;

        public CreateModel(ProductManager productManager, StorageManager storageManager,
            SelectListHelpers selectListHelpers, ProductMovementHelpers productMovementHelpers, StatisticManager statisticManager, UserManager<InventoryManagementUser> userManager)
        {
            _productManager = productManager;
            _storageManager = storageManager;
            _selectListHelpers = selectListHelpers;
            _productMovementHelpers = productMovementHelpers;
            _statisticManager = statisticManager;
            _userManager = userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }
        [TempData]
        public string StatusMessage1 { get; set; }
        public InventoryManagementUser MyUser { get; set; }
        [BindProperty]
        public InventoryTracker InventoryTracker { get; set; } = default!;
        [BindProperty]
        public Models.Storage DefaultStorage { get; set; }
        public SelectList StorageSelectList { get; set; }
        public SelectList ProductSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            MyUser = await _userManager.GetUserAsync(User);
            var products = await _productManager.GetProductsAsync(false);
            var storages = await _storageManager.GetStoragesAsync(false);

            DefaultStorage = await _storageManager.GetDefaultStorageAsync();

            StorageSelectList = await _selectListHelpers.GenerateStorageSelectListAsync(DefaultStorage.Id);
            ProductSelectList = await _selectListHelpers.GenerateProductSelectListBasedOfStorageAsync(DefaultStorage.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            MyUser = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            int productId = (int)InventoryTracker.ProductId;
            int fromStorageId = (int)DefaultStorage.Id;
            int toStorageId = (int)InventoryTracker.StorageId;
            int quantity = (int)InventoryTracker.Quantity;

            if (productId <= 0 || fromStorageId <= 0 || toStorageId <= 0 || quantity <= 0)
            {
                return RedirectToPage("./Create", new { id = InventoryTracker.Id });
            }

			var moveResult = await _productMovementHelpers.MoveProductAsync(productId, fromStorageId, toStorageId, quantity);

			if (!moveResult.Success)
			{
				StatusMessage = !string.IsNullOrEmpty(moveResult.Message) ? moveResult.Message : "Förflyttning lyckades ej!";
				return RedirectToPage("./Create", new { id = InventoryTracker.Id });
			}

			StatusMessage1 = !string.IsNullOrEmpty(moveResult.Message) ? moveResult.Message : "Förflyttning lyckades!";
			return RedirectToPage("./Create");

		}
	}
}


