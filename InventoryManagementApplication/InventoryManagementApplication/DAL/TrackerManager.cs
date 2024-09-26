using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InventoryManagementApplication.DAL
{
	public class TrackerManager
	{
        private static Uri BaseAddress = new Uri("https://localhost:44353/");

		public InventoryTracker InventoryTracker { get; set; }

		public List<InventoryTracker> InventoryTrackers { get; set; }


        public async Task<InventoryTracker> CreateTrackerAsync(InventoryTracker tracker)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                var json = JsonSerializer.Serialize(tracker);
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/InventoryTrackers/", httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating tracker: {response.StatusCode} - {errorMessage}");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryTracker>(responseString);
            }
        }
        public async Task DeleteTrackerAsync(int id)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;

				var response = await client.DeleteAsync($"api/InventoryTrackers/{id}");
			}
		}
		public async Task<Models.InventoryTracker> GetOneTrackerAsync(int id)
		{
           

            using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage responseProducts = await client.GetAsync($"api/InventoryTrackers/{id}");

                if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();

					// blir fel här
					 InventoryTracker = JsonSerializer.Deserialize<Models.InventoryTracker>(responseString);
					
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

				var option = new JsonSerializerOptions()
				{
                    ReferenceHandler = ReferenceHandler.Preserve
                };

				if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();
					List<Models.InventoryTracker> trackers = JsonSerializer.Deserialize<List<Models.InventoryTracker>>(responseString, option);
					InventoryTrackers = trackers.ToList();
				}

				return InventoryTrackers;
			}
		}

        public async Task<List<InventoryTracker>> GetTrackerByProductOrStorageAsync(int? productId, int? storageId)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

				string uri = $"api/InventoryTrackers/{productId}/{storageId}";
                HttpResponseMessage responseProducts = await client.GetAsync(uri);

                if (responseProducts.IsSuccessStatusCode)
                {
                    string responseString = await responseProducts.Content.ReadAsStringAsync();
                    List<Models.InventoryTracker> tracker = JsonSerializer.Deserialize<List<Models.InventoryTracker>>(responseString);
                    InventoryTrackers = tracker;
                }
            }

            return InventoryTrackers;
        }

		public async Task<InventoryTracker> GetTrackerByProductAndStorageAsync(int? productId, int? storageId)
		{
			if(productId.HasValue && storageId.HasValue)
			{
				var inventoryTrackers = await GetTrackerByProductOrStorageAsync(productId, storageId);

				if(inventoryTrackers.Count == 1)
				{
					var inventoryTracker = inventoryTrackers[0];

					return inventoryTracker;
				}			
            }
            return null;
        }

        public async Task EditTrackerAsync(InventoryTracker? tracker)
		{
			var existingTracker = (await GetAllTrackersAsync()).Where(x => x.Id == tracker.Id).SingleOrDefault();

			if(existingTracker != null)
			{
                using (var client = new HttpClient())
                {
                    client.BaseAddress = BaseAddress;

                    var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(tracker), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync($"api/InventoryTrackers/{tracker.Id}", content);
                }
            }
			else
			{
				using (var client = new HttpClient())
				{
					client.BaseAddress = BaseAddress;

					var json = JsonSerializer.Serialize(tracker);

					StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

					HttpResponseMessage responseMessage = await client.PostAsync("api/InventoryTrackers/", httpContent);
				}
			}

		}
	}
}
