using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementApplication.DAL;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementApplication.Pages
{
    // Bara admin har tillgång
  [Authorize(Roles = "Admin")]
    public class UsersRolesModel : PageModel
    {
        private readonly UserManager<InventoryManagementUser> _userManager;
        private readonly RoleManager<InventoryManagementRole> _roleManager;
        private readonly DAL.UserManager _userManagerDAL;

        public UsersRolesModel(UserManager<InventoryManagementUser> userManager, RoleManager<InventoryManagementRole> roleManager,
            DAL.UserManager userManagerDAL)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userManagerDAL = userManagerDAL;
        }

        public List<UserWithRoleViewModel> UsersWithRoles { get; set; } = new List<UserWithRoleViewModel>();
        public string LoggedInUserName { get; set; }
        
        public class UserWithRoleViewModel
        {
            public string EmployeeNumber { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string RoleName { get; set; }
            public DateTime Created { get; set; }
            public DateTime Updated { get; set; }
            public bool IsDeleted { get; set; }
            public string Id { get; set; }
        }

        [BindProperty]
        public bool IsDeletedToggle { get; set; }
        public int UserCount { get; set; }

        //Hämtar användare
        public async Task OnGetAsync(bool isDeletedToggle = false)
        {
            IsDeletedToggle = isDeletedToggle;
            UsersWithRoles = await LoadUsers(IsDeletedToggle);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = await _userManagerDAL.GetOneUserAsync(userId);

            LoggedInUserName = loggedInUser != null ? FormatUserName(loggedInUser) : string.Empty;
            UserCount = UsersWithRoles.Count();
        }

        public async Task<IActionResult> OnPostAsync(int buttonId)
        {
            if(buttonId == 1)
            {
                IsDeletedToggle = !IsDeletedToggle;
                return RedirectToPage("/UsersRoles", new { isDeletedToggle = IsDeletedToggle });
            }
            else
            {
                return Page();
            }
        }

        private async Task<List<UserWithRoleViewModel>> LoadUsers(bool? isDeleted)
        {
            var users = await _userManagerDAL.GetAllUsersAsync(isDeleted);
            var orderUsers = users.OrderBy(x => x.LastName).ToList();
            UsersWithRoles = new List<UserWithRoleViewModel>();
            
            foreach (var user in orderUsers)
            {
                UsersWithRoles.Add(await CreateUserWithRoleViewModel(user));
            }

            return UsersWithRoles;
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