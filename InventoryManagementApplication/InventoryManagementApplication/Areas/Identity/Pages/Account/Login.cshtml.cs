using System.ComponentModel.DataAnnotations;
using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.DAL;

namespace InventoryManagementApplication.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<InventoryManagementUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager _userManager;
        private readonly StatisticManager _statisticManager;

        public LoginModel(SignInManager<InventoryManagementUser> signInManager, ILogger<LoginModel> logger, UserManager userManager, StatisticManager statisticManager)
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
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

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
                if(user.IsDeleted == true)
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
