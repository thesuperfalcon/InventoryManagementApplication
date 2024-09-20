using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace InventoryManagementApplication.Pages
{
    public class StatisticModel : PageModel
    {
        //private readonly InventoryManagementApplicationContext _context;

        //public StatisticPageModel(InventoryManagementApplicationContext context)
        //{
        //    _context = context;
        //}

        private static Uri BaseAddress = new Uri("https://localhost:44353/");


        public static async Task<List<StatisticDto>> GetStatisticsAsync()
        {
            var statistics = new List<StatisticDto>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                HttpResponseMessage responseMessage = await client.GetAsync("api/Statistics/");
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    statistics = JsonSerializer.Deserialize<List<StatisticDto>>(responseString);
                }
                return statistics;
            }
        }


        public List<StatisticDto> Statistics { get; set; } = new List<StatisticDto>();


        public async Task OnGetAsync()
        {
            Statistics = await GetStatisticsAsync();
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
