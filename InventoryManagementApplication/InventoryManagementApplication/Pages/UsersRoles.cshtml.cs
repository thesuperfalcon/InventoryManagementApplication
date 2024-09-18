using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementApplication.Pages
{
    // Bara admin har tillgång
    [Authorize(Roles = "Admin")]
    public class UsersRolesModel : PageModel
    {
        private readonly UserManager<InventoryManagementUser> _userManager;
        private readonly RoleManager<InventoryManagementRole> _roleManager;

        public UsersRolesModel(UserManager<InventoryManagementUser> userManager, RoleManager<InventoryManagementRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<UserWithRoleViewModel> UsersWithRoles { get; set; }
        public string LoggedInUserName { get; set; }

        public class UserWithRoleViewModel
        {
            public string EmployeeNumber { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string RoleName { get; set; }
            public DateTime Created { get; set; }
            public DateTime Updated { get; set; }
            public string Id { get; set; }
        }

        //Hämtar användare
        public async Task OnGetAsync()
        {
            var users = _userManager.Users.ToList();
            UsersWithRoles = new List<UserWithRoleViewModel>();

            foreach (var user in users)
            {
                UsersWithRoles.Add(await CreateUserWithRoleViewModel(user));
            }

            var loggedInUser = await _userManager.GetUserAsync(User);
            LoggedInUserName = loggedInUser != null ? FormatUserName(loggedInUser) : string.Empty;
        }

        //Skapar en viewmodel för en given användare
        private async Task<UserWithRoleViewModel> CreateUserWithRoleViewModel(InventoryManagementUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.Count > 0 ? roles[0] : "Användare";

            return new UserWithRoleViewModel
            {
                EmployeeNumber = user.EmployeeNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleName = roleName,
                Created = user.Created,
                Updated = user.Updated,
                Id = user.Id
            };
        }

        //Formattering av användarens namn
        private string FormatUserName(InventoryManagementUser user)
        {
            return $"{user.FirstName} {user.LastName} ({user.EmployeeNumber})";
        }
    }
}