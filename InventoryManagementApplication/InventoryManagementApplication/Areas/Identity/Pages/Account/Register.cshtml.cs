﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
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

namespace InventoryManagementApplication.Areas.Identity.Pages.Account
{
    [Authorize]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<InventoryManagementUser> _signInManager;
        private readonly UserManager<InventoryManagementUser> _userManager;
        private readonly IUserStore<InventoryManagementUser> _userStore;
        private readonly IUserEmailStore<InventoryManagementUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly DAL.UserManager _userManagerDAL;


        public RegisterModel(
            UserManager<InventoryManagementUser> userManager,
            IUserStore<InventoryManagementUser> userStore,
            SignInManager<InventoryManagementUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            DAL.UserManager userManagerDAL)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _userManagerDAL = userManagerDAL;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 

        [TempData]
        public string StatusMessage { get; set; }


        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            //[Required]
            //[EmailAddress]
            //[Display(Name = "Email")]
            //public string Email { get; set; }


            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            //[Required]
            //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            //[DataType(DataType.Password)]
            //[Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            //[DataType(DataType.Password)]
            //[Display(Name = "Confirm password")]
            //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            //public string ConfirmPassword { get; set; }


            // Tillagt manuellt

            [Required]
            [Display(Name = "Förnamn")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Efternamn")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Anställningsnummer")]
            public string EmployeeNumber { get; set; }


        }

        //public static string GenerateRandomEmplyeeNumber()
        //{
        //    Random random = new Random();

        //    int employeeNumber = random.Next(1000, 10000);

        //    return employeeNumber.ToString();
        //}

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
                //await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                //var result = await _userManager.CreateAsync(user, Input.Password);

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

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.FirstName, returnUrl = returnUrl });
                    //}
                    //else
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);
                    //    return LocalRedirect(returnUrl);
                    //}

                    StatusMessage = "Användare har lagts till!<br>" +
                                    $"Namn: {user.FirstName} {user.LastName}<br>" +
                                    $"Anställningsnummer: {user.EmployeeNumber}<br>" +
                                    $"Användarnamn: {user.UserName}<br>" +
                                    $"Lösenord: {Input.Password}";

                    return LocalRedirect(returnUrl);
                }
                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}
            }

            // If we got this far, something failed, redisplay form
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

        private IUserEmailStore<InventoryManagementUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<InventoryManagementUser>)_userStore;
        }
    }
}
