// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;

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

            //[Required]
            //[EmailAddress]
            //public string Email { get; set; }


            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }


            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string UserName { get; set; }

        }

        public List<UserStatisticsViewModel> MovementPerPerson { get; set; } = new List<UserStatisticsViewModel>(); 
        
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            var users = await _userManager.GetAllUsersAsync(false);
            returnUrl ??= Url.Content("~/");

            var statistics = await _statisticManager.GetAllStatisticsAsync();


            var personList = await _userManager.GetAllUsersAsync(false);
            foreach (var person in personList)
            {
                var movementsByUser = statistics.Where(stat => stat.UserId == person.Id);


                //var currentWeek = GetCurrentWeekNumber();
                //movementsByUser = movementsByUser
                //    .Where(stat => stat.Moved.HasValue &&
                //                   GetWeekNumber(stat.Moved.Value) == currentWeek &&
                //                   DateTime.Now.Year == stat.Moved.Value.Year);


                //movementsByUser = movementsByUser
                //    .Where(stat => stat.Moved.HasValue &&
                //                   IsSameDay(stat.Moved.Value));

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

            // Clear the existing external cookie to ensure a clean login process
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
                    ModelState.AddModelError(string.Empty, "Anställningsnummer finns ej!");
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

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
