using System.Text.Json;
namespace InventoryManagementApplication.DAL
{
    public class ProductManager
    {
        private static Uri BaseAddress = new Uri("https://localhost:44353/");
        public static async Task<List<Models.Product>> GetAllProductsAsync()
        {

            List<Models.Product> products = new List<Models.Product>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync("/api/Products");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    products = JsonSerializer.Deserialize<List<Models.Product>>(responseString);
                }

                return products;
            }

        }
    }
}
