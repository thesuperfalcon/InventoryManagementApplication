using InventoryManagementApplication.Models;
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
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"api/Storages/{storage.Id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error updating storage: {errorResponse}");
                }
            }
        }
    }
}
