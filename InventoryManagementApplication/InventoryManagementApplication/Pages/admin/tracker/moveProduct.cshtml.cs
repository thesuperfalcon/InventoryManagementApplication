using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Helpers;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace InventoryManagementApplication.Pages.admin.tracker
{
    [Authorize]
    public class MoveProductModel : PageModel
    {
        private readonly UserManager<InventoryManagementUser> _userManager;
        private readonly SelectListHelpers _selectListHelpers;
        private readonly TrackerManager _trackerManager;
        private readonly ProductMovementHelpers _productMovementHelpers;
        private readonly StatisticManager _statisticManager;

        public MoveProductModel(UserManager<InventoryManagementUser> userManager,
            SelectListHelpers selectListHelpers, TrackerManager trackerManager,
            ProductMovementHelpers productMovementHelpers,
            StatisticManager statisticManager)
        {
            _userManager = userManager;
            _selectListHelpers = selectListHelpers;
            _trackerManager = trackerManager;
            _productMovementHelpers = productMovementHelpers;
            _statisticManager = statisticManager;
        }

		[TempData]
		public string StatusMessage { get; set; }

		[BindProperty]
		public InventoryTracker InventoryTracker { get; set; } = default!;

		[BindProperty]
		public InventoryTracker SelectedInventoryTracker { get; set; }

		public SelectList StorageSelectList { get; set; }
		public SelectList ProductSelectList { get; set; }
		public InventoryManagementUser MyUser { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			MyUser = await _userManager.GetUserAsync(User);

			if (!id.HasValue)
			{
				return RedirectToPage("./Index");
			}

			SelectedInventoryTracker = await _trackerManager.GetOneTrackerAsync(id.Value);
			if (SelectedInventoryTracker == null)
			{
				return RedirectToPage("./Index");
			}

			StorageSelectList = await _selectListHelpers.GenerateStorageSelectListAsync(SelectedInventoryTracker.StorageId);
			ProductSelectList = await _selectListHelpers.GenerateProductSelectListAsync();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			MyUser = await _userManager.GetUserAsync(User);

			if (!ModelState.IsValid)
			{
				return Page();
			}

			int productId = (int)SelectedInventoryTracker.ProductId;
			int fromStorageId = (int)SelectedInventoryTracker.StorageId;
			int toStorageId = (int)InventoryTracker.StorageId;
			int quantity = (int)InventoryTracker.Quantity;

			string errorMessage = ValidateInputs(productId, fromStorageId, toStorageId, quantity);
			if (errorMessage != null)
			{
				StatusMessage = errorMessage;
				return RedirectToPage("./moveProduct", new { id = SelectedInventoryTracker.Id });
			}

			var moveResult = await _productMovementHelpers.MoveProductAsync(productId, fromStorageId, toStorageId, quantity);
			if (!moveResult.Success)
			{
				StatusMessage = !string.IsNullOrEmpty(moveResult.Message) ? moveResult.Message : "Förflyttning lyckades ej!";
				return RedirectToPage("./moveProduct", new { id = SelectedInventoryTracker.Id });
			}

			StatusMessage = !string.IsNullOrEmpty(moveResult.Message) ? moveResult.Message : "Förflyttning lyckades!";
			await _statisticManager.GetValueFromStatisticAsync(MyUser.Id, fromStorageId, toStorageId, productId, quantity, null);
			return RedirectToPage("./moveProduct", new { id = SelectedInventoryTracker.Id });
		}

		private string ValidateInputs(int productId, int fromStorageId, int toStorageId, int quantity)
		{
			if (productId <= 0)
				return "Ogiltigt produkt-ID. Vänligen ange ett giltigt produkt-ID.";
			if (fromStorageId <= 0)
				return "Ogiltigt från-lager-ID. Vänligen ange ett giltigt från-lager.";
			if (toStorageId <= 0)
				return "Ogiltigt till-lager-ID. Vänligen ange ett giltigt till-lager.";
			if (quantity <= 0)
				return "Ogiltig kvantitet. Vänligen ange en positiv kvantitet.";

			return null;
		}
	}
}