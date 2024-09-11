using System.Text.Json;
namespace InventoryManagementApplication.DAL
{
    public class StorageManager
    {
        private static Uri BaseAddress = new Uri("https://localhost:44353/");
        public static async Task<List<Models.Storage>> GetAllStoragesAsync()
        {

            List<Models.Storage> storages = new List<Models.Storage>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync("/api/Storages");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    storages = JsonSerializer.Deserialize<List<Models.Storage>>(responseString);
                }

                return storages;
            }

        }
    }
}
