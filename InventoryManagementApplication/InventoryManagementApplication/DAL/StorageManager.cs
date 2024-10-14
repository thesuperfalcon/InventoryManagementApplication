using Azure;
using InventoryManagementApplication.Models;
using Microsoft.VisualBasic;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
	public class StorageManager
	{
        private static Uri BaseAddress = new Uri("https://localhost:44353/");
        public Storage Storage { get; set; }
        public List<Storage> Storages { get; set; }


        public async Task CreateStorageAsync(Storage storage)
        {
            if (storage != null)
            {
                Storage = storage;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = BaseAddress;
                    var json = JsonSerializer.Serialize(Storage);

                    StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("api/Storages/", httpContent);
                }
            }
            else
            {
                Console.WriteLine("Error! Abort!");
            }

        }

        public async Task <Storage> GetDefaultStorageAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                string uri = "api/Storages/DefaultStorage";

                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Storage storage = JsonSerializer.Deserialize<Storage>(responseString);
                    Storage = storage;
                }

                return Storage;
            }
        }

        public async Task<bool> CheckStorageName(string storageName)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                string uri = $"api/Storages/ByStorageName/{storageName}";

                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<bool>(responseString);
                }
                return false;
            }
        }


        public async Task<List<Storage>> GetStoragesAsync(bool? isDeleted)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                string uri = "api/Storages/";
                if(isDeleted != null)
                {
                    uri += isDeleted == false ? "ExistingStorages" : "DeletedStorages";
                }
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    List<Models.Storage> storages = JsonSerializer.Deserialize<List<Models.Storage>>(responseString);
                    Storages = storages.ToList();
                }

                return Storages;
            }
        }

        public async Task<Storage> GetStorageByIdAsync(int? id, bool? isDeleted)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                string uri = "api/Storages/";
                if (isDeleted != null)
                {
                    uri += isDeleted == false ? $"ExistingStorages/{id}" : $"DeletedStorages/{id}";
                }
                else
                {
                    uri += id;
                }
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    var storage = JsonSerializer.Deserialize<Models.Storage>(responseString);
                    Storage = storage;
                }

                return Storage;
            }
        }

        public async Task<List<Storage>> SearchStoragesAsync(string? inputValue)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                string uri = "api/Storages/SearchStorages";


                if (!string.IsNullOrEmpty(inputValue))
                {
                    uri += $"?inputValue={inputValue}";
                }

                HttpResponseMessage responseStorages = await client.GetAsync(uri);

                List<Storage> storages = new List<Storage>();

                if (responseStorages.IsSuccessStatusCode)
                {
                    string responseString = await responseStorages.Content.ReadAsStringAsync();
                    storages = JsonSerializer.Deserialize<List<Storage>>(responseString) ?? new List<Storage>();
                }

                return storages;
            }
        }

        public async Task DeleteStorageAsync(int? id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                var response = await client.DeleteAsync($"api/Storages/{id}");
            }
        }

        public async Task EditStorageAsync(Storage storage)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                var json = JsonSerializer.Serialize(storage);
				Console.WriteLine($"Serialized JSON: {json}");

				var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"api/Storages/{storage.Id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error updating storage: {errorResponse}");
					Console.WriteLine($"Status Code: {response.StatusCode}");
					Console.WriteLine($"Reason Phrase: {response.ReasonPhrase}");
					Console.WriteLine($"Response Headers: {response.Headers}");
				}
            }
        }
    }
}
