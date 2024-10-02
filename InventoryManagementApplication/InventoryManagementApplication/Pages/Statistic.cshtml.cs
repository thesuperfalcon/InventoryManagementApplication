using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagementApplication.Pages
{
    public class StatisticModel : PageModel
    {
        private readonly StatisticManager _statisticManager;
        private readonly UserManager _userManager;
        public StatisticModel(StatisticManager statisticManager, UserManager userManager)
        {
            _statisticManager = statisticManager;
            _userManager = userManager;
        }

        [BindProperty]
        public bool StatisticSwitch { get; set; }
        public List<Statistic> Statistics { get; set; }
        public List<Statistic> MovementPerPerson { get; set; } = new List<Statistic>();
        public async Task OnGetAsync(bool statisticSwitch)
        {

            var statistics = await _statisticManager.GetAllStatisticsAsync();
            Statistics = statistics.ToList();
            StatisticSwitch = statisticSwitch;

                var personList = await _userManager.GetAllUsersAsync();
                foreach (var person in personList)
                {
                    var quantityPerPerson = new Statistic
                    {
                        EmployeeNumber = person.EmployeeNumber,
                        Quantity = await GetTotalQuantityByUserAsync(Statistics, person.Id)
                    };
                    MovementPerPerson.Add(quantityPerPerson);
                }
                if (MovementPerPerson.Count > 0)
                {
                    MovementPerPerson.OrderBy(x => x.Quantity);
                }
        }

        public async Task<int> GetTotalQuantityByUserAsync(List<Statistic> statistics, string userId)
        {
            return statistics
                .Where(stat => stat.UserId == userId && stat.Quantity.HasValue) // Filtrerar på användarens ID och att Quantity inte är null
                .Sum(stat => stat.Quantity.Value); // Summerar alla Quantity-värden  

        }
    }
}
