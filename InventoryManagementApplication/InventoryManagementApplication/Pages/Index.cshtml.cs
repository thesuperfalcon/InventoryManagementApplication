using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Areas.Identity.Data;
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
        public List<InventoryManagementUser> Users { get; set; }

        [BindProperty]
        public List<UserStatisticsViewModel> MovementPerPerson{ get; set; } = new List<UserStatisticsViewModel>();
        public async Task OnGetAsync()
        {
            var statistics = await _statisticManager.GetAllStatisticsAsync();
           
                
            var personList = await _userManager.GetAllUsersAsync();
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
                       
                    };
                    MovementPerPerson.Add(userStatistics);
                }
            }

            Users = await _userManager.GetAllUsersAsync();

            var user = new InventoryManagementUser();

            if (user == null)
            {

                //Users = Users
                //    .Where(u => u.FirstName != null && u.LastName.Contains()
                //    .ToList();
            }
            

        }
    }
}
