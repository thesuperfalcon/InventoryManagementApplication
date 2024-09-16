using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
	public class InventoryTrackerManager
	{
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public static async Task<List<Models.InventoryTracker>> GetAllInventoryTrackers()
		{

			List<Models.InventoryTracker> inventoryTrackers = new List<Models.InventoryTracker>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage response = await client.GetAsync("/api/InventoryTrackers");
				if (response.IsSuccessStatusCode)
				{
					string responseString = await response.Content.ReadAsStringAsync();
					inventoryTrackers = JsonSerializer.Deserialize<List<Models.InventoryTracker>>(responseString);
				}

				return inventoryTrackers;
			}

		}
	}
}
