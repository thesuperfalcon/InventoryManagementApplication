using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventoryManagementApplication.Pages.admin.storage
{
    [Authorize]
    public class DeleteModel : PageModel
	{
		private readonly StorageManager _storageManager;
		private readonly TrackerManager _trackerManager;
		private readonly UserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteModel(StorageManager storageManager, TrackerManager trackerManager, UserManager userManager, IHttpContextAccessor httpContextAccessor)
		{
			_storageManager = storageManager;
			_trackerManager = trackerManager;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		[BindProperty]
		public Storage Storage { get; set; } = default!;
		public List<InventoryTracker> Trackers { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			
			var storage = await _storageManager.GetStorageByIdAsync(id, null);

			if (storage == null)
			{
				return NotFound();
			}		

			bool isAuthorized = await CheckStorageDeletedAndUserAuthorize(storage.IsDeleted == true);

			if (!isAuthorized)
			{
				return Forbid();
			}

            Storage = storage;

            return Page();
        }
		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var storage = await _storageManager.GetStorageByIdAsync(id, null);
			Trackers = await _trackerManager.GetAllTrackersAsync();
			Trackers = Trackers.Where(x => x.StorageId == storage.Id).ToList();

			var sum = Trackers.Sum(x => x.Quantity);
			if (sum > 0)
			{
				return RedirectToPage("./Delete", new { id = storage.Id });
			}
			if (storage != null)
			{
				await _storageManager.DeleteStorageAsync(storage);
			}

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

			if(user == null)
			{
				return false;
			}

			var roles = await _userManager.GetUserRoleAsync(userId);
			bool isAdmin = roles.Contains("Admin");

            return isAdmin || !isDeleted;
        }
    }
}
