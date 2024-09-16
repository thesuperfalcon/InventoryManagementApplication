using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
	public class StatisticManager
	{
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public static async Task<List<Models.Statistic>> GetAllStatistics()
		{

			List<Models.Statistic> statistics = new List<Models.Statistic>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage response = await client.GetAsync("/api/Statistics");
				if (response.IsSuccessStatusCode)
				{
					string responseString = await response.Content.ReadAsStringAsync();
					statistics = JsonSerializer.Deserialize<List<Models.Statistic>>(responseString);
				}

				return statistics;
			}

		}
	}
}
