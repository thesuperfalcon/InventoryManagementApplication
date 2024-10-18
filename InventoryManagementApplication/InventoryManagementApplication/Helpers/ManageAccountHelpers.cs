using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Helpers
{
    public class ManageAccountHelpers : PageModel
    {
        private readonly SignInManager<InventoryManagementUser> _signInManager;
        private readonly ILogger<ManageAccountHelpers> _logger;
        private readonly UserManager _userManager;
        private readonly StatisticManager _statisticManager;

        public ManageAccountHelpers(SignInManager<InventoryManagementUser> signInManager, ILogger<ManageAccountHelpers> logger, UserManager userManager, StatisticManager statisticManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _statisticManager = statisticManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public List<UserStatisticsViewModel> MovementPerPerson { get; set; } = new List<UserStatisticsViewModel>();

        public class InputModel
        {

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string UserName { get; set; }

        }

        public async Task<string> GenerateUniqueEmployeeNumber()
        {
            Random random = new Random();
            string employeeNumber;
            bool exists;

            do
            {
                employeeNumber = random.Next(0, 10000).ToString("D4");

                var users = await _userManager.GetAllUsersAsync(null);
                exists = users.Any(x => x.EmployeeNumber == employeeNumber);

            }
            while (exists);

            return employeeNumber;
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            var statistics = await _statisticManager.GetAllStatisticsAsync();

            var personList = await _userManager.GetAllUsersAsync(false);
            if(personList != null)
            {
                foreach (var person in personList)
                {
                    var movementsByUser = statistics.Where(stat => stat.UserId == person.Id);

                    if (movementsByUser.Any())
                    {
                        var totalMovements = movementsByUser.Count();
                        var totalQuantity = movementsByUser.Sum(stat => stat.Quantity ?? 0);

                        var userStatistics = new UserStatisticsViewModel
                        {
                            EmployeeNumber = person.EmployeeNumber,
                            TotalMovements = totalMovements,
                            TotalQuantity = totalQuantity,
                            RecentMovements = null
                        };
                        MovementPerPerson.Add(userStatistics);
                    }
                }
            }
            

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _signInManager.UserManager.Users.SingleOrDefaultAsync(u => u.UserName == Input.UserName);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Användarnamnet finns ej!");
                    return Page();
                }
                if (user.IsDeleted == true)
                {
                    ModelState.AddModelError(string.Empty, "Användaren existerar inte!");
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Fel lösenord");
                    return Page();
                }
            }

            return Page();
        }
    }
}
