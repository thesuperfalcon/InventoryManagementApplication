using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace InventoryManagementApplication.Areas.Identity.Pages.Account.Manage
{

    [Authorize]
    public class ProfilePicModel : PageModel
    {
        private readonly DAL.UserManager _userManager;
        public ProfilePicModel(DAL.UserManager userManager)
        {
            _userManager = userManager;
        }
        public List<string> ProfilePics { get; set; }

        [BindProperty] public string SelectedProfilePic { get; set; }

        public InventoryManagementUser SelectedUser { get; set; }
        public async Task OnGetAsync()
        {
			string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			SelectedUser = await _userManager.GetOneUserAsync(userId);

			ProfilePics = await _userManager.GetPicUrlAsync();


        }

        public async Task<IActionResult> OnPostAsync()
        {
			string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			SelectedUser = await _userManager.GetOneUserAsync(userId);

            SelectedUser.ProfilePic = SelectedProfilePic;
            await _userManager.EditUserAsync(SelectedUser, null);

            return RedirectToPage("./ProfilePic");
        }
    }
}
