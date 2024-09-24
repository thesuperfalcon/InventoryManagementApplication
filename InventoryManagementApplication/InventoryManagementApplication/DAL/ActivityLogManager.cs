using InventoryManagementApplication.Models;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
	public class ActivityLogManager
	{
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public ActivityLog ActivityLog { get; set; }
		public List<ActivityLog> ActivityLogs { get; set; }

		public async Task CreateActivityLogAsync(ActivityLog activityLog)
		{
			if (activityLog != null)
			{
				ActivityLog = activityLog;
				using (var client = new HttpClient())
				{
					client.BaseAddress = BaseAddress;
					var json = JsonSerializer.Serialize(ActivityLog);

					StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PostAsync("api/ActivityLogs/", httpContent);
				}
			}
			else
			{
				Console.WriteLine("Error! Abort!");
			}

		}
		public async Task<List<ActivityLog>> GetAllActivityLogssAsync()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage responseProducts = await client.GetAsync("api/ActivityLogs");

				if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();
					List<Models.ActivityLog> activityLogs = JsonSerializer.Deserialize<List<Models.ActivityLog>>(responseString);
					ActivityLogs = activityLogs.ToList();
				}

				return ActivityLogs;
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

		public async Task DeleteActivityLogAsync(int? id)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;

				var response = await client.DeleteAsync($"api/ActivityLogs/{id}");
			}
		}

		public async Task EditActivityLogAsync(ActivityLog? activityLog)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				if (activityLog != null)
				{
					ActivityLog = activityLog;
					var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(ActivityLog), Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PutAsync($"api/ActivityLogs/{ActivityLog.Id}", content);
				}



			}
		}
	}
}
