using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using System.Globalization;

namespace InventoryManagementApplication.Helpers
{
	public class StatisticLeaderboardHelpers
	{
		private readonly UserManager _userManager;
		private readonly StatisticManager _statisticManager;
		public StatisticLeaderboardHelpers(StatisticManager statisticManager, UserManager userManager)
		{
			_statisticManager = statisticManager;
			_userManager = userManager;
		}

		public List<UserStatisticsViewModel> MovementPerPerson { get; set; } = new List<UserStatisticsViewModel>();

		public async Task<List<UserStatisticsViewModel>> CreateLeaderboardList(List<Statistic>? statistics)
		{
			if(statistics == null || statistics.Count >= 0)
			{
				statistics = await _statisticManager.GetAllStatisticsAsync();
			}
			var personList = await _userManager.GetAllUsersAsync(null);
			var currentWeek = GetCurrentWeekNumber();

			foreach (var person in personList)
			{
				var movementsByUser = statistics.Where(stat => stat.UserId == person.Id &&
					stat.Moved.HasValue &&
					GetWeekNumber(stat.Moved.Value) == currentWeek &&
					DateTime.Now.Year == stat.Moved.Value.Year);

				if (movementsByUser.Any())
				{
					var totalMovements = movementsByUser.Count();
					var totalQuantity = movementsByUser.Sum(stat => stat.Quantity ?? 0);
					var recentMovements = movementsByUser.OrderByDescending(stat => stat.Moved).Take(5).ToList();

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
			return MovementPerPerson;
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
