using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static InventoryManagementApplication.Pages.UsersRolesModel;
using InventoryManagementApplication.DAL;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Identity;
using InventoryManagementApplication.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace InventoryManagementApplication.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
            
        private readonly UserManager _userManager;
        private readonly DAL.UserManager _userManagerDal;
        private readonly StatisticManager _statisticManager;
        public IndexModel(UserManager userManager, DAL.UserManager userManagerDal, StatisticManager statisticManager)
        {
            _userManager = userManager;
            _userManagerDal = userManagerDal;
            _statisticManager = statisticManager;
        }
       
        public async Task OnGetAsync()
        {
          
        }
    }
}
