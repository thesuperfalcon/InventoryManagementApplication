using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagementApplication.Pages
{
    [Authorize]
    public class OverviewModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly StorageManager _storageManager;
        private readonly ProductManager _productManager;
        private readonly StatisticManager _statisticManager;
        public OverviewModel(UserManager userManager, StorageManager storageManager, ProductManager productManager, StatisticManager statisticManager)
        {
            _userManager = userManager;
            _storageManager = storageManager;
            _productManager = productManager;
            _statisticManager = statisticManager;
        }
        public int ProductQuantity { get; set; }
        public int StorageQuantity { get; set; }
        public Product ProductHighestQuantity { get; set; }
        public Storage StorageHighestQuantity { get; set; }
        public Product NewestProduct { get; set; }
        public Storage NewestStorage { get; set; }
        public UserStatisticsViewModel UserStatistic { get; set; }
        public Statistic NewestStatistic { get; set; }
        public InventoryManagementUser User { get; set; }
        public async Task OnGetAsync()
        {
            var products = await _productManager.GetProductsAsync(false);

            var storages = await _storageManager.GetStoragesAsync(false);

            var statistics = await _statisticManager.GetAllStatisticsAsync();

            var personList = await _userManager.GetAllUsersAsync(null);

            User = personList.MaxBy(x => x.Created);

            ProductQuantity = products.Count;
            StorageQuantity = storages.Count;

            ProductHighestQuantity = products.MaxBy(x => x.CurrentStock);

            StorageHighestQuantity = storages.MaxBy(x => x.CurrentStock);

            NewestProduct = products.MaxBy(x => x.Created);

            NewestStorage = storages.MaxBy(x => x.Created);

            NewestStatistic = statistics.MaxBy(x => x.Moved);

            var userStatistics = new List<UserStatisticsViewModel>();

            foreach (var person in personList)
            {
                var movementsByUser = statistics.Where(stat => stat.UserId == person.Id);


                if (movementsByUser.Any())
                {
                    var totalMovements = movementsByUser.Count();
                    var totalQuantity = movementsByUser.Sum(stat => stat.Quantity ?? 0);

                    var recentMovements = movementsByUser
                        .OrderByDescending(stat => stat.Moved)
                        .Take(5)
                        .ToList();

                    var userStatistic = new UserStatisticsViewModel
                    {
                        EmployeeNumber = person.EmployeeNumber,
                        TotalMovements = totalMovements,
                        TotalQuantity = totalQuantity,
                        RecentMovements = recentMovements
                    };
                    userStatistics.Add(userStatistic);
                }
            }

            UserStatistic = userStatistics.MaxBy(x => x.TotalQuantity);
        }
    }
}
