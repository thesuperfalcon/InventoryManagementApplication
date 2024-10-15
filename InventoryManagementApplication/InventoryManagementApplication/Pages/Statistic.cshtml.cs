using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace InventoryManagementApplication.Pages
{
    [Authorize]
    public class StatisticModel : PageModel
    {
        private readonly StatisticManager _statisticManager;
        private readonly UserManager _userManager;

        public StatisticModel(StatisticManager statisticManager, UserManager userManager)
        {
            _statisticManager = statisticManager;
            _userManager = userManager;
        }


        public List<Statistic> Statistics { get; set; }
        public List<UserStatisticsViewModel> MovementPerPerson { get; set; } = new List<UserStatisticsViewModel>();


        [BindProperty(SupportsGet = true)]
        public string EmployeeNumber { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            var statistics = await _statisticManager.GetAllStatisticsAsync();
            Statistics = statistics
                .OrderByDescending(x => x.Moved)  
                .ToList();   
            
            

            var personList = await _userManager.GetAllUsersAsync(null);
            foreach (var person in personList)
            {

                var movementsByUser = statistics.Where(stat => stat.UserId == person.Id);

                
                var currentWeek = GetCurrentWeekNumber();
                movementsByUser = movementsByUser
                    .Where(stat => stat.Moved.HasValue &&
                                   GetWeekNumber(stat.Moved.Value) == currentWeek &&
                                   DateTime.Now.Year == stat.Moved.Value.Year);
                

                //movementsByUser = movementsByUser
                //    .Where(stat => stat.Moved.HasValue &&
                //                   IsSameDay(stat.Moved.Value));

                if (movementsByUser.Any())
                {
                    var totalMovements = movementsByUser.Count();
                    var totalQuantity = movementsByUser.Sum(stat => stat.Quantity ?? 0);

                    var recentMovements = movementsByUser
                        .OrderByDescending(stat => stat.Moved)
                        .Take(5)
                        .ToList();

                    var userStatistics = new UserStatisticsViewModel
                    {
                        EmployeeNumber = person.EmployeeNumber,
                        TotalMovements = totalMovements,
                        TotalQuantity = totalQuantity,
                        RecentMovements = recentMovements
                    };
                    MovementPerPerson.Add(userStatistics);
                }
            }


            if (MovementPerPerson.Count > 0)
            {
                MovementPerPerson = MovementPerPerson.OrderByDescending(x => x.TotalQuantity).ToList();
            }

            return Page();
        }

        public async Task<int> GetTotalQuantityByUserAsync(List<Statistic> statistics, string userId)
        {
            return statistics
                .Where(stat => stat.UserId == userId && stat.Quantity.HasValue)
                .Sum(stat => stat.Quantity.Value);
        }

        public static int GetCurrentWeekNumber()
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        public static int GetWeekNumber(DateTime date)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        public static bool IsSameDay(DateTime date)
        {
            return DateTime.Now.Date == date.Date; 
        }
    }
}
