using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
	public class ActivityLogManager
	{
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public static async Task<List<Models.ActivityLog>> GetAllActivityLogs()
		{

			List<Models.ActivityLog> activityLogs = new List<Models.ActivityLog>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage response = await client.GetAsync("/api/ActivityLogs");
				if (response.IsSuccessStatusCode)
				{
					string responseString = await response.Content.ReadAsStringAsync();
					activityLogs = JsonSerializer.Deserialize<List<Models.ActivityLog>>(responseString);
				}

				return activityLogs;
			}

		}
	}
}
