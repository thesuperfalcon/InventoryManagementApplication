using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Helpers;
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
        private readonly StatisticLeaderboardHelpers _statisticLeaderboardHelpers;

        public StatisticModel(StatisticManager statisticManager, UserManager userManager, StatisticLeaderboardHelpers statisticLeaderboardHelpers)
        {
            _statisticManager = statisticManager;
            _userManager = userManager;
            _statisticLeaderboardHelpers = statisticLeaderboardHelpers;
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

            MovementPerPerson = await _statisticLeaderboardHelpers.CreateLeaderboardList(null);
            

            return Page();
        }
    }
}
