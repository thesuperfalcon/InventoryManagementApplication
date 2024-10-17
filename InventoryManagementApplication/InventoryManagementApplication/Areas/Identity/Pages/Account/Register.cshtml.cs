#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using InventoryManagementApplication.Helpers;

namespace InventoryManagementApplication.Areas.Identity.Pages.Account
{
    [Authorize]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<InventoryManagementUser> _signInManager;
        private readonly UserManager<InventoryManagementUser> _userManager;
        private readonly IUserStore<InventoryManagementUser> _userStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly DAL.UserManager _userManagerDAL;
        private readonly ManageAccountHelpers _helper;


        public RegisterModel(
            UserManager<InventoryManagementUser> userManager,
            IUserStore<InventoryManagementUser> userStore,
            SignInManager<InventoryManagementUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            DAL.UserManager userManagerDAL,
            ManageAccountHelpers helper)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _userManagerDAL = userManagerDAL;
            _helper = helper;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            public string Password { get; set; }
          
            [Required(ErrorMessage = "Förnamn är obligatoriskt.")]
            [Display(Name = "Förnamn")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Efternamn är obligatoriskt.")]
            [Display(Name = "Efternamn")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Anställningsnummer är obligatoriskt.")]
            [BindProperty]
            [Display(Name = "Anställningsnummer")]
            public string EmployeeNumber { get; set; }


        }

        public async Task<IActionResult> OnGetCheckEmployeeNumberExists(string employeeNumber)
        {
            bool exists = await _userManager.Users.AnyAsync(u => u.EmployeeNumber == employeeNumber);
            return new JsonResult(!exists);
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }




        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            returnUrl ??= Url.Content("/Identity/Account/Register");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName; 
                user.EmployeeNumber = Input.EmployeeNumber;
                user.ProfilePic = "images/ProfileAvatars/profile_avatar_1.png";

                string firstTwoLettersFirstName = user.FirstName.Length >= 2 ? user.FirstName.Substring(0, 2).ToLower() : user.FirstName.ToLower();
                string firstTwoLettersLastName = user.LastName.Length >= 2 ? user.LastName.Substring(0, 2).ToLower() : user.LastName.ToLower();
                user.UserName = $"{firstTwoLettersFirstName}{firstTwoLettersLastName}{user.EmployeeNumber.ToLower()}";

                //För att visa korrekt datum i användarinfomationen
                user.Created = DateTime.Now;
                user.Updated = DateTime.Now;

                Input.Password = "Admin123!";

                await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);

                var result = await _userManagerDAL.RegisterUserAsync(user);
                
                if (result != null)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                  
                    StatusMessage = "Användare har lagts till!<br>" +
                                    $"Namn: {user.FirstName} {user.LastName}<br>" +
                                    $"Anställningsnummer: {user.EmployeeNumber}<br>" +
                                    $"Användarnamn: {user.UserName}<br>" +
                                    $"Lösenord: {Input.Password}";

                    return LocalRedirect(returnUrl);
                }
              
            }

            return Page();
        }

        private InventoryManagementUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<InventoryManagementUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(InventoryManagementUser)}'. " +
                    $"Ensure that '{nameof(InventoryManagementUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
         
        // This method goes to registerUser.js
        public async Task<IActionResult> OnGetGenerateEmployeeNumberAsync()
        {
            string generatedNumber = await _helper.GenerateUniqueEmployeeNumber();
            return new JsonResult(new { employeeNumber = generatedNumber });
        }
    }
}
