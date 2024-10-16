using InventoryManagementApplication.Models;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
	public class ProductManager
	{
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public Product Product { get; set; } = new Product();
		public List<Product> Products { get; set; } = new List<Product>();

        public async Task CreateProductAsync(Product product)
		{
			if(product != null)
			{
				Product = product;
				using (var client = new HttpClient())
				{
					client.BaseAddress = BaseAddress;
					var json = JsonSerializer.Serialize(Product);

					StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PostAsync("api/Products/", httpContent);
				}
			}
			else
			{
				Console.WriteLine("Error! Abort!");
			}
			
		}
		public async Task<List<Product>> GetProductsAsync(bool? isDeleted)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				string uri = "/api/Products/";
				if (isDeleted != null)
				{
					uri += isDeleted == false ? "ExistingProducts" : "DeletedProducts";
				}
				HttpResponseMessage responseProducts = await client.GetAsync(uri);

				if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();
					List<Product> products = JsonSerializer.Deserialize<List<Product>>(responseString);
					if (products.Count > 0)
					{
						Products = products;
					}
				}

				return Products;
			}
		}

		public async Task<bool> CheckProductName(string name)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				string uri = $"/api/Products/ProductByName/{name}";
				HttpResponseMessage response = await client.GetAsync(uri);
				if (response.IsSuccessStatusCode)
				{
					string responseString = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<bool>(responseString);
				}
			}
			return false;	
        }

        public async Task<Product> GetProductByIdAsync(int? id, bool? isDeleted)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				string uri = "api/Products/";
                if (isDeleted != null)
                {
					uri += isDeleted == false ? $"ExistingProducts/{id}" : $"DeletedProducts/{id}"; 
                }
				else
				{
					uri += id;
				}
                HttpResponseMessage responseProducts = await client.GetAsync(uri);

				if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();
					var product = JsonSerializer.Deserialize<Product>(responseString);
					Product = product;
				}

				return Product;
			}
		}

		public async Task DeleteProductAsync(int? id)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;

				var response = await client.DeleteAsync($"api/Products/{id}");
			}
		}

        public async Task<List<Product>> SearchProductsAsync(string? inputValue)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                string uri = "api/Products/SearchProducts";


                if (!string.IsNullOrEmpty(inputValue))
                {
					uri += $"?inputValue={inputValue}";

				}

                HttpResponseMessage responseProducts = await client.GetAsync(uri);

                List<Product> products = new List<Product>();

                if (responseProducts.IsSuccessStatusCode)
                {
                    string responseString = await responseProducts.Content.ReadAsStringAsync();
                    products = JsonSerializer.Deserialize<List<Product>>(responseString) ?? new List<Product>();
                }

                return products;
            }
        }
        public async Task EditProductAsync(Product? product)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				if (product != null)
				{
					Product = product;
					var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(Product), Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PutAsync($"api/products/{Product.Id}", content);
				}
			}
		}
	}
}