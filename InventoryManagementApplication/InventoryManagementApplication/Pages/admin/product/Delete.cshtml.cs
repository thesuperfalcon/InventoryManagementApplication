using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventoryManagementApplication.Pages.admin.product
{
	[Authorize]
	public class DeleteModel : PageModel
	{
		private readonly ProductManager _productManager;
		private readonly StorageManager _storageManager;
		private readonly TrackerManager _trackerManager;
		private readonly LogManager _activityLogManager;
		private readonly UserManager _userManager; // Inject UserManager
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteModel(ProductManager productManager,
			StorageManager storageManager, TrackerManager trackerManager, LogManager activityLogManager,
			UserManager userManager, IHttpContextAccessor httpContextAccessor) // Include UserManager in the constructor
		{
			_productManager = productManager;
			_storageManager = storageManager;
			_trackerManager = trackerManager;
			_activityLogManager = activityLogManager;
			_userManager = userManager; // Assign to private field
			_httpContextAccessor = httpContextAccessor;
		}

		[BindProperty]
		public Product Product { get; set; } = default!;
		public List<InventoryTracker> InventoryTrackers { get; set; } = new List<InventoryTracker>();
		public List<Storage> Storages { get; set; } = new List<Storage>();

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Product = await _productManager.GetProductByIdAsync(id, null);

			if (Product == null)
			{
				return NotFound();
			}

            bool isAuthorized = await CheckStorageDeletedAndUserAuthorize(Product.IsDeleted == true);

            if (!isAuthorized)
            {
                return Forbid();
            }

            return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
            var product = await _productManager.GetProductByIdAsync(id.Value, null);
            if (id == null)
			{
				return NotFound();
			}
            
			if (product == null)
			{
				return NotFound();
			}

            var trackers = await _trackerManager.GetAllTrackersAsync();
			InventoryTrackers = trackers.Where(x => x.ProductId == id).ToList();

			var storages = await _storageManager.GetStoragesAsync(false);

			foreach (var tracker in InventoryTrackers)
			{
				Models.Storage storage = storages.FirstOrDefault(s => s.Id == tracker.StorageId);
				if (storage != null)
				{
					await _trackerManager.DeleteTrackerAsync(tracker.Id);
					var trackerStorage = await _storageManager.GetStorageByIdAsync(storage.Id, false);
					trackerStorage.CurrentStock -= tracker.Quantity;

					await _storageManager.EditStorageAsync(trackerStorage);
				}
			}

			// Log the deletion action or take any other necessary actions with the user info
			// Example: await _activityLogManager.LogActivityAsync(product, EntityState.Deleted, user);

			await _productManager.DeleteProductAsync(product);

			return RedirectToPage("./Index");
		}

        private async Task<bool> CheckStorageDeletedAndUserAuthorize(bool isDeleted)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            var user = await _userManager.GetOneUserAsync(userId);

            if (user == null)
            {
                return false;
            }

            var roles = await _userManager.GetUserRoleAsync(userId);
            bool isAdmin = roles.Contains("Admin");

            return isAdmin || !isDeleted;
        }
    }
}