using InventoryManagementApplication.Models;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
	public class TrackerManager
	{
        private static Uri BaseAddress = new Uri("https://localhost:44353/");

		public InventoryTracker InventoryTracker { get; set; }

		public List<InventoryTracker> InventoryTrackers { get; set; }


		public async Task CreateTrackerAsync(InventoryTracker tracker)
		{
			InventoryTracker = tracker;
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				var json = JsonSerializer.Serialize(InventoryTracker);

				StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.PostAsync("api/InventoryTrackers/", httpContent);
			}
		}
		public async Task DeleteTrackerAsync(int? id)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;

				var response = await client.DeleteAsync($"api/InventoryTrackers/{id}");
			}
		}
		public async Task<Models.InventoryTracker> GetOneTrackerAsync(int? id)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage responseProducts = await client.GetAsync($"api/InventoryTrackers/{id}");

				if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();
					Models.InventoryTracker tracker = JsonSerializer.Deserialize<Models.InventoryTracker>(responseString);
					InventoryTracker = tracker;
				}			
			}

			return InventoryTracker;
		}

		public async Task<List<InventoryTracker>> GetAllTrackersAsync()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage responseProducts = await client.GetAsync("api/InventoryTrackers/");

				if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();
					List<Models.InventoryTracker> trackers = JsonSerializer.Deserialize<List<Models.InventoryTracker>>(responseString);
					InventoryTrackers = trackers.ToList();
				}

				return InventoryTrackers;
			}
		}

		public async Task EditTrackerAsync(InventoryTracker? tracker)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;

				var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(tracker), Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.PutAsync($"api/InventoryTrackers/{tracker.Id}", content);
			}
		}
	}
}
