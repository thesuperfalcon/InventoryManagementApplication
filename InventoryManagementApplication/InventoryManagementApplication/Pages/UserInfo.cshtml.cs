using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementApplication.Pages
{
    //Endast admin har åtkomst
    [Authorize(Roles = "Admin")]
    public class UserInfoModel : PageModel
    {
        private readonly UserManager<InventoryManagementUser> _userManager;
        private readonly RoleManager<InventoryManagementRole> _roleManager;
        public UserInfoModel(UserManager<InventoryManagementUser> userManager, RoleManager<InventoryManagementRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            AvailableRoles = new List<string>();
        }


        [BindProperty]
        public InventoryManagementUser SelectedUser { get; set; }

        [BindProperty]
        public string SelectedRole { get; set; }
        public List<string> AvailableRoles { get; set; }

        private async Task PopulateAvailableRolesAsync()
        {
            if (_roleManager == null)
            {
                Console.WriteLine("RoleManager är null.");
                return;
            }

            var roles = await _roleManager.Roles.ToListAsync();
            if (roles == null || roles.Count == 0)
            {
                Console.WriteLine("Inga roller hittades i databasen.");
                roles = new List<InventoryManagementRole>();
            }

            AvailableRoles = roles.Select(r => r.Name).ToList();
            AvailableRoles.Insert(0, "Användare");

            if (string.IsNullOrEmpty(SelectedRole))
            {
                SelectedRole = AvailableRoles.FirstOrDefault();
            }
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("Användar-ID saknas.");
            }

            SelectedUser = await _userManager.FindByIdAsync(userId);
            if (SelectedUser == null)
            {
                return NotFound("Användaren kunde inte hittas.");
            }

            await PopulateAvailableRolesAsync();

            var userRoles = await _userManager.GetRolesAsync(SelectedUser);
            SelectedRole = userRoles.FirstOrDefault() ?? "Användare"; // Om ingen roll, sätt "Användare"

            return Page();
        }


        public async Task<IActionResult> OnPostAssignRoleAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(SelectedRole))
            {
                await PopulateAvailableRolesAsync();
                return NotFound("Användar-ID eller roll saknas.");
            }

            ModelState.Clear();

            SelectedUser = await _userManager.FindByIdAsync(userId);

            if (SelectedUser == null)
            {
                return NotFound("Användaren kunde inte hittas.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateAvailableRolesAsync();
                return Page();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Användaren kunde inte hittas.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Contains(SelectedRole))
            {
                TempData["SuccessMessage"] = "Ingen förändring i roll.";
                return RedirectToPage(new { userId });
            }

            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (SelectedRole != "Användare")
            {
                var result = await _userManager.AddToRoleAsync(user, SelectedRole);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    await PopulateAvailableRolesAsync();
                    return Page();
                }
            }

            TempData["SuccessMessage"] = "Användaren har tilldelats en ny roll";
            return RedirectToPage(new { userId });
        }


        public async Task<IActionResult> OnPostResetPasswordAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("Användar-ID saknas.");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Användaren kunde inte hittas.");
            }

            // Hårdkodat lösenord vid reset
            var newPassword = "Admin123!";
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (!resetResult.Succeeded)
            {
                foreach (var error in resetResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                await PopulateAvailableRolesAsync();
                return Page();
            }

            TempData["SuccessMessage"] = "Lösenordet har återställts!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSaveAsync(string userId)
        {
            Console.WriteLine($"OnPostSaveAsync called with userId: {userId}");

            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Model error: {modelError.ErrorMessage}");
                }

                await PopulateAvailableRolesAsync();
                return Page();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return NotFound("Användaren kunde inte hittas.");
            }

            Console.WriteLine($"Updating user: {user.FirstName}, {user.LastName}, {user.EmployeeNumber}");

            user.FirstName = SelectedUser.FirstName;
            user.LastName = SelectedUser.LastName;
            user.EmployeeNumber = SelectedUser.EmployeeNumber;

            if (user.Created == DateTime.MinValue)
            {
                user.Created = DateTime.Now;
            }

            user.Updated = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Update error: {error.Description}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await PopulateAvailableRolesAsync();
                return Page();
            }

            Console.WriteLine("User updated successfully.");

            await PopulateAvailableRolesAsync();
            return Redirect("/UsersRoles");
        }
        public async Task<IActionResult> OnPostDeleteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Användaren hittades inte.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                await PopulateAvailableRolesAsync();
                return Page();
            }

            return Redirect("/UsersRoles");

        }
    }
}