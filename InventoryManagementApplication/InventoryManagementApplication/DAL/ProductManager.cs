using InventoryManagementApplication.Models;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
	public class ProductManager
	{
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public Product Product { get; set; }
		public List<Product> Products { get; set; }

		public async Task CreateProductAsync(Product product)
		{
			if(product != null)
			{
				Product = product;
				using (var client = new HttpClient())
				{
					client.BaseAddress = BaseAddress;
					var json = JsonSerializer.Serialize(Product);

					StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PostAsync("api/Products/", httpContent);
				}
			}
			else
			{
				Console.WriteLine("Error! Abort!");
			}
			
		}
		public async Task<List<Product>> GetAllProductsAsync()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage responseProducts = await client.GetAsync("api/Products");

				if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();
					List<Models.Product> products = JsonSerializer.Deserialize<List<Models.Product>>(responseString);
					Products = products.ToList();
				}

				return Products;
			}
		}

		public async Task<Product> GetOneProductAsync(int? id)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage responseProducts = await client.GetAsync($"api/Products/{id.Value}");

				if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();
					var product = JsonSerializer.Deserialize<Models.Product>(responseString);
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
