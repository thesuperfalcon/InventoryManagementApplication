using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InventoryManagementApplication.Models;
using InventoryManagementApplication.Helpers;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagementApplication.Pages
{
	[Authorize]
	public class IndexModel : PageModel
	{
		private readonly UserManager _userManager;
		private readonly DAL.UserManager _userManagerDal;
		private readonly StatisticLeaderboardHelpers _statisticLeaderboardHelpers;

		public IndexModel(
			UserManager userManager,
			DAL.UserManager userManagerDal,
			StatisticLeaderboardHelpers statisticLeaderboardHelpers)
		{
			_userManager = userManager;
			_userManagerDal = userManagerDal;
			_statisticLeaderboardHelpers = statisticLeaderboardHelpers;
		}

		public List<InventoryManagementUser> Users { get; set; }

		[BindProperty]
		public List<UserStatisticsViewModel> MovementPerPerson { get; set; } = new List<UserStatisticsViewModel>();

		public async Task OnGetAsync()
		{

            MovementPerPerson = await _statisticLeaderboardHelpers.CreateLeaderboardList(null);
		}
	}
}