using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryManagementApplication.Pages
{
    //Endast admin har åtkomst
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
             _signInManager = signInManager;
            AvailableRoles = new List<string>();
        }


 
               [BindProperty]
        public InventoryManagementUser SelectedUser { get; set; }
      

        [BindProperty]
        public string SelectedRole { get; set; }
        public List<string> AvailableRoles { get; set; }

    

        private async Task PopulateAvailableRolesAsync()
        {
            if (_roleManagerDal == null)
            {
                Console.WriteLine("RoleManager är null.");
                return;
            }

            var roles = await _roleManagerDal.GetAllRolesAsync();
            if (roles == null || roles.Count == 0)
            {
                Console.WriteLine("Inga roller hittades i databasen.");
                roles = new List<InventoryManagementRole>();
            }

            AvailableRoles = roles.Select(r => r.RoleName).ToList();
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
            
            SelectedUser = await _userManagerDal.GetOneUserAsync(userId);
            //SelectedUser = await _userManager.FindByIdAsync(userId);
            if (SelectedUser == null)
            {
                return NotFound("Användaren kunde inte hittas.");
            }

            await PopulateAvailableRolesAsync();

            var userRoles = await _roleManagerDal.GetAllRolesAsync();
            if(SelectedUser.RoleId == userRoles[0].Id)
            {
                SelectedRole = userRoles[0].RoleName;
            }
            else
            {
                SelectedRole = "Användare";
            }
            //var userRoles = await _userManager.GetRolesAsync(SelectedUser);
            //SelectedRole = userRoles.FirstOrDefault() ?? ; // Om ingen roll, sätt "Användare"

            return Page();
        }


//Ser till att det alltid finns en admin
        public async Task<int> GetAdminCountAsync()
{
    var admins = await _userManager.GetUsersInRoleAsync("Admin");
    return admins.Count;
}



        public async Task<IActionResult> OnPostAssignRoleAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(SelectedRole))
            {
                await PopulateAvailableRolesAsync();
                return NotFound("Användar-ID eller roll saknas.");
            }

            ModelState.Clear();

            SelectedUser = await _userManagerDal.GetOneUserAsync(userId);
            //SelectedUser = await _userManager.FindByIdAsync(userId);

            if (SelectedUser == null)
            {
                return NotFound("Användaren kunde inte hittas.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateAvailableRolesAsync();
                return Page();
            }

            var user = await _userManagerDal.GetOneUserAsync(userId);
            //var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Användaren kunde inte hittas.");
            }

            var getRoles = await _roleManagerDal.GetAllRolesAsync();
            var currentRoles = getRoles.Where(r => r.Id == user.RoleId).Select(r => r.RoleName).ToList();
            
            //var currentRoleName = currentRoles.Select(r => r.Name).ToList();
            //var currentRoles = await _userManager.GetRolesAsync(user);


            if(currentRoles.Contains(SelectedRole)) 
            {
                TempData["SuccessMessage"] = "Ingen förändring i roll.";
                return RedirectToPage(new { userId });
            }
            //if (currentRoles.Contains(SelectedRole))
            //{
            //    TempData["SuccessMessage"] = "Ingen förändring i roll.";
            //    return RedirectToPage(new { userId });
            //}

            await _roleManagerDal.RemoveFromRoleAsync(user, currentRoles);

            if (SelectedRole != "Användare")
            {
                var result = await _roleManagerDal.AddToRoleAsync(user, SelectedRole);

                //var result = await _userManager.AddToRoleAsync(user, SelectedRole);

                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Kunde inte tilldela rollen till användaren");
                    //foreach (var error in result.Errors)
                    //{
                    //    ModelState.AddModelError(string.Empty, error.Description);
                    //}

                    await PopulateAvailableRolesAsync();
                    return Page();
                }
            }

   // Om den inloggade användaren ändrar sin egen roll till "Användare"
    var loggedInUser = await _userManager.GetUserAsync(User);
    if (loggedInUser != null && loggedInUser.Id == userId && SelectedRole == "Användare")
    {
        // Uppdatera den inloggade användarens säkerhetsanspråk för att reflektera den nya rollen
        await _signInManager.RefreshSignInAsync(loggedInUser);

        // Omdirigera till startsidan
        return RedirectToPage("/Index");
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
            var user = await _userManagerDal.GetOneUserAsync(userId);
            //var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Användaren kunde inte hittas.");
            }

            // Hårdkodat lösenord vid reset
            //var newPassword = "Admin123!";
            //var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            //var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            var resetResult = await _userManagerDal.ResetPassword(user, null);
            if (!resetResult)
            {
                ModelState.AddModelError(string.Empty, "Gick ej att återställa lösenord");
                //foreach (var error in resetResult.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}

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
            var user = await _userManagerDal.GetOneUserAsync(userId);
            //var user = await _userManager.FindByIdAsync(userId);
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

       if (user.Updated == DateTime.MinValue || user.Updated == default(DateTime))
{
    user.Updated = DateTime.Now;
}

            var result = await _userManagerDal.EditUserAsync(user, null);
           // var result = await _userManager.UpdateAsync(user);

            if (!result)
            {
                Console.WriteLine($"Update error: Gick ej att uppdatera användare");
                ModelState.AddModelError(string.Empty, "Gick ej att uppdatera användare");
                //foreach (var error in result.Errors)
                //{
                //    Console.WriteLine($"Update error: {error.Description}");
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}
                await PopulateAvailableRolesAsync();
                return Page();
            }

            Console.WriteLine("User updated successfully.");

            await PopulateAvailableRolesAsync();
            return Redirect("/UsersRoles");
        }
        public async Task<IActionResult> OnPostDeleteAsync(string userId)
        {
          /*  var user = await _userManagerDal.GetOneUserAsync(userId);
            //var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Användaren hittades inte.");
            }*/



 var currentUser = await _userManager.GetUserAsync(User); // Hämta den inloggade användaren
    if (currentUser == null)
    {
        return NotFound("Det gick inte att hämta den inloggade användaren.");
    }

    if (currentUser.Id == userId)
    {
        // Förhindra att den inloggade användaren kan radera sitt eget konto
        ModelState.AddModelError(string.Empty, "Du kan inte radera ditt eget konto medan du är inloggad.");
        await PopulateAvailableRolesAsync();
        return Page();
    }
   var user = await _userManagerDal.GetOneUserAsync(userId);
    if (user == null)
    {
        return NotFound("Användaren hittades inte.");
    }

            var result = await _userManagerDal.DeleteUserAsync(user.Id);
            //var result = await _userManager.DeleteAsync(user);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Gick ej att ta bort användare");
                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}

                await PopulateAvailableRolesAsync();
                return Page();
            }

            return Redirect("/UsersRoles");

        }
    }
}