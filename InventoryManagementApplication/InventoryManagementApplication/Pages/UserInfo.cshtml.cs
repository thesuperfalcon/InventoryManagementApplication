using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;

namespace InventoryManagementApplication.Pages
{
    // Endast admin har åtkomst
    [Authorize(Roles = "Admin")]
    public class UserInfoModel : PageModel
    {
        private readonly UserManager<InventoryManagementUser> _userManager;
        private readonly RoleManager<InventoryManagementRole> _roleManager;
        private readonly SignInManager<InventoryManagementUser> _signInManager;
        private readonly DAL.UserManager _userManagerDal;
        private readonly DAL.RoleManager _roleManagerDal;

        public UserInfoModel(UserManager<InventoryManagementUser> userManager, RoleManager<InventoryManagementRole> roleManager,
             SignInManager<InventoryManagementUser> signInManager, DAL.UserManager userManagerDal, DAL.RoleManager roleManagerDal)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userManagerDal = userManagerDal;
            _roleManagerDal = roleManagerDal;
            AvailableRoles = new List<string>();
        }

        [BindProperty]
        public InventoryManagementUser SelectedUser { get; set; }

        [BindProperty]
        public string SelectedRole { get; set; }
        public List<string> AvailableRoles { get; set; }

        private async Task PopulateAvailableRolesAsync()
        {
            var roles = await _roleManagerDal.GetAllRolesAsync() ?? new List<InventoryManagementRole>();
            AvailableRoles = roles.Select(r => r.RoleName).ToList();
            AvailableRoles.Insert(0, "Användare");
            SelectedRole ??= AvailableRoles.FirstOrDefault();
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound("Användar-ID saknas.");

            SelectedUser = await _userManagerDal.GetOneUserAsync(userId);
            if (SelectedUser == null)
                return NotFound("Användaren kunde inte hittas.");

            var currentRoles = await _userManager.GetRolesAsync(SelectedUser);
            SelectedRole = currentRoles.Contains("Admin") ? "Admin" : "Användare";

            await PopulateAvailableRolesAsync();
            return Page();
        }

        private async Task<bool> AssignRoleAsync(InventoryManagementUser user, string newRole)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);

            if (newRole == "Admin" && !currentRoles.Contains("Admin"))
                return (await _userManager.AddToRoleAsync(user, "Admin")).Succeeded;

            if (newRole == "Användare" && currentRoles.Contains("Admin"))
            {
                if (await GetAdminCountAsync() <= 1)
                    return false; // Måste finnas minst en admin

                return (await _userManager.RemoveFromRoleAsync(user, "Admin")).Succeeded;
            }

            return false;
        }

        private async Task HandleLoggedInUserAsync(string userId, bool isRemovingAdmin)
        {
            var loggedInUserId = _userManager.GetUserId(User);
            if (userId == loggedInUserId && isRemovingAdmin)
            {
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(await _userManager.FindByIdAsync(userId), isPersistent: false);
            }
        }

        public async Task<IActionResult> OnPostAssignRoleAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(SelectedRole))
                return NotFound("Användar-ID eller roll saknas.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Användaren kunde inte hittas.");

            var roleChanged = await AssignRoleAsync(user, SelectedRole);
            if (!roleChanged)
            {
                TempData["SuccessMessage"] = "Ingen rolländring gjordes.";
                return RedirectToPage(new { userId });
            }

            await HandleLoggedInUserAsync(userId, SelectedRole == "Användare");
            TempData["SuccessMessage"] = SelectedRole == "Admin" ? "Användaren har tilldelats adminrollen." : "Användaren har nu ingen adminroll.";
            return RedirectToPage(new { userId });
        }

        public async Task<int> GetAdminCountAsync() => (await _userManager.GetUsersInRoleAsync("Admin")).Count;

        public async Task<IActionResult> OnPostResetPasswordAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound("Användar-ID saknas.");

            var user = await _userManagerDal.GetOneUserAsync(userId);
            if (user == null)
                return NotFound("Användaren kunde inte hittas.");

            var resetResult = await _userManagerDal.ResetPassword(user, null, null, true);
            if (!resetResult)
            {
                ModelState.AddModelError(string.Empty, "Gick ej att återställa lösenord");
                await PopulateAvailableRolesAsync();
                return Page();
            }

            TempData["SuccessMessage"] = "Lösenordet har återställts!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSaveAsync(string userId)
        {
            if (!ModelState.IsValid)
            {
                await PopulateAvailableRolesAsync();
                return Page();
            }

            var user = await _userManagerDal.GetOneUserAsync(userId);
            if (user == null)
                return NotFound("Användaren kunde inte hittas.");

            user.FirstName = SelectedUser.FirstName;
            user.LastName = SelectedUser.LastName;
            user.UserName = $"{user.FirstName[..2].ToLower()}{user.LastName[..2].ToLower()}{user.EmployeeNumber.ToLower()}";
            user.NormalizedUserName = _userManager.NormalizeName(user.UserName);
            user.Created = user.Created == DateTime.MinValue ? DateTime.Now : user.Created;
            user.Updated = DateTime.Now;

            var result = await _userManagerDal.EditUserAsync(user, null, null, false);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Gick ej att uppdatera användare");
                await PopulateAvailableRolesAsync();
                return Page();
            }

            await PopulateAvailableRolesAsync();
            return Redirect("/UsersRoles");
        }

        public async Task<IActionResult> OnPostDeleteAsync(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Id == userId)
            {
                ModelState.AddModelError(string.Empty, "Du kan inte radera ditt eget konto medan du är inloggad.");
                await PopulateAvailableRolesAsync();
                return Page();
            }

            var user = await _userManagerDal.GetOneUserAsync(userId);
            if (user == null)
                return NotFound("Användaren hittades inte.");

            var result = await _userManagerDal.DeleteUserAsync(user.Id);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Gick ej att ta bort användare");
                await PopulateAvailableRolesAsync();
                return Page();
            }

            TempData["ConfirmationMessage"] = "Användaren har raderats. Du kommer att omdirigeras till användaröversikten om några sekunder.";
            TempData["RedirectLink"] = "/UsersRoles";
            return Partial("_EditLoggedInUserConfirmation", TempData["ConfirmationMessage"]);
        }
        public async Task<IActionResult> OnPostRecreateUserAsync(string userID)
        {
            var selectedUser = await _userManagerDal.GetOneUserAsync(userID);

            if(selectedUser != null && selectedUser.IsDeleted == true)
            {
                selectedUser.IsDeleted = false;
                await _userManagerDal.EditUserAsync(selectedUser, null, null, false);
            }
            return RedirectToPage("/UsersRoles");
        }
    }
}