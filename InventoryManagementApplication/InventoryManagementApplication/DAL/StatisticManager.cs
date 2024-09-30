using InventoryManagementApplication.Models;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
	public class StatisticManager
	{
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public Statistic Statistic { get; set; }
		public List<Statistic> Statistics { get; set; }
		public StatisticDto StatisticDto { get; set; }
		public List<StatisticDto> StatisticsDto { get; set; }

		public async Task CreateStatisticAsync(StatisticDto statistic)
		{
			if (statistic != null)
			{
				StatisticDto = statistic;
				using (var client = new HttpClient())
				{
					client.BaseAddress = BaseAddress;
					var json = JsonSerializer.Serialize(StatisticDto);

					StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PostAsync("api/Statistics/", httpContent);
				}
			}
			else
			{
				Console.WriteLine("Error! Abort!");
			}

		}
		public async Task<List<StatisticDto>> GetAllStatisticsAsync()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage responseProducts = await client.GetAsync("api/Statistics/");

				if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();
					List<Models.StatisticDto> statistics = JsonSerializer.Deserialize<List<Models.StatisticDto>>(responseString);
					StatisticsDto = statistics.ToList();
				}

				return StatisticsDto;
			}
		}

		//public async Task<Product> GetOneProductAsync(int? id)
		//{
		//	using (var client = new HttpClient())
		//	{
		//		client.BaseAddress = BaseAddress;
		//		HttpResponseMessage responseProducts = await client.GetAsync($"api/Products/{id.Value}");

		//		if (responseProducts.IsSuccessStatusCode)
		//		{
		//			string responseString = await responseProducts.Content.ReadAsStringAsync();
		//			var product = JsonSerializer.Deserialize<Models.Product>(responseString);
		//			Product = product;
		//		}

		//		return Product;
		//	}
		//}

		public async Task DeleteStatisticAsync(int? id)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;

				var response = await client.DeleteAsync($"api/Statistics/{id}");
			}
		}

		public async Task EditStatisticAsync(Statistic? statistic)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				if (statistic != null)
				{
					Statistic = statistic;
					var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(Statistic), Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PutAsync($"api/Statistics/{Statistic.Id}", content);
				}



			}
		}
	}
}
