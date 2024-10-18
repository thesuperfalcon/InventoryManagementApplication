using Azure;
using InventoryManagementApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
	public class StorageManager
	{
        private readonly LogManager _logManager;
        public StorageManager(LogManager logManager)
        {
            _logManager = logManager;
        }
        private static Uri BaseAddress = new Uri("https://localhost:44353/");
        public Storage Storage { get; set; } = new Storage();
        public List<Storage> Storages { get; set; } = new List<Storage>();

        public async Task CreateStorageAsync(Storage storage)
        {
            if (storage != null)
            {
                Storage = storage;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = BaseAddress;
                    var json = JsonSerializer.Serialize(Storage);

                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("api/Storages/", httpContent);

                    if(response.IsSuccessStatusCode)
                    {
                        var createdStorage = await response.Content.ReadFromJsonAsync<Storage>();

                        await _logManager.LogActivityAsync(createdStorage, EntityState.Added);
                    }
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

        public async Task DeleteStorageAsync(Storage storage)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                var response = await client.DeleteAsync($"api/Storages/{storage.Id}");

                if (response.IsSuccessStatusCode)
                {
                    await _logManager.LogActivityAsync(storage, EntityState.Deleted);
                }
            }
        }

        public async Task EditStorageAsync(Storage updatedStorage)
        {
            var originalStorage = await GetStorageByIdAsync(updatedStorage.Id, null);
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                if (updatedStorage != null)
                {
                    var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(updatedStorage), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync($"api/Storages/{updatedStorage.Id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        if (originalStorage != null)
                        {
                            await _logManager.LogActivityAsync(updatedStorage, EntityState.Modified, originalStorage);
                        }
                    }
                }
            }
        }
    }
}
