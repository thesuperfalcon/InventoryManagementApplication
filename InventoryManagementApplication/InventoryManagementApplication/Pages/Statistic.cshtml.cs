using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace InventoryManagementApplication.Pages
{
    public class StatisticModel : PageModel
    {
        private readonly StatisticManager _statisticManager;
        public StatisticModel(StatisticManager statisticManager)
        {
            _statisticManager = statisticManager;
        }

		//private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public List<Statistic> Statistics { get; set; }
		public async Task OnGetAsync()
		{
			var statistics = await _statisticManager.GetAllStatisticsAsync();
			Statistics = statistics.ToList();
			//    .Include(x => x.InitialStorage)
			//    .Include(x => x.DestinationStorage)
			//    .Include(x => x.Product)
			//    .Include(x => x.User)
			//    .ToListAsync();

			//Statistics = Statistics.Where(statistic =>
			//statistic?.Product != null &&
			//statistic.DestinationStorage != null &&
			//statistic.InitialStorage != null &&
			//statistic.User != null).ToList();

		}



        
    }
}
//public static async Task<List<Statistic>> GetStatisticsAsync()
//      {
//          var statistics = new List<Statistic>();


//          using (var client = new HttpClient())
//          {
//              client.BaseAddress = BaseAddress;

//              HttpResponseMessage responseMessage = await client.GetAsync("api/Statistics/");
//              if (responseMessage.IsSuccessStatusCode)
//              {
//                  string responseString = await responseMessage.Content.ReadAsStringAsync();
//                  statistics = JsonSerializer.Deserialize<List<Statistic>>(responseString);
//              }

//          }
//          return statistics;
//}

