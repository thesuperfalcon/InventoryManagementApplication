using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
	public class StatisticManager
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly StorageManager _storageManager;
		private readonly ProductManager _productManager;
		private readonly UserManager _userManager;
        public StatisticManager(StorageManager storageManager, ProductManager productManager, UserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
			_storageManager = storageManager;
            _productManager = productManager;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
        }
        private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public async Task CreateStatisticAsync(Statistic statistic)
		{
			if (statistic != null)
			{
				using (var client = new HttpClient())
				{
					client.BaseAddress = BaseAddress;
					var json = JsonSerializer.Serialize(statistic);

					StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PostAsync("api/Statistics/", httpContent);
				}
			}
			else
			{
				Console.WriteLine("Error! Abort!");
			}

		}
		public async Task<List<Statistic>> GetAllStatisticsAsync()
		{
			var statistics = new List<Statistic>();
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage responseProducts = await client.GetAsync("api/Statistics/");

				if (responseProducts.IsSuccessStatusCode)
				{
					string responseString = await responseProducts.Content.ReadAsStringAsync();
					statistics = JsonSerializer.Deserialize<List<Models.Statistic>>(responseString);
				}

				return statistics;
			}
		}

		public async Task DeleteStatisticAsync(int? id)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;

				var response = await client.DeleteAsync($"api/Statistics/{id}");
			}
		}

		public async Task EditStatisticAsync(Statistic? statistic)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				if (statistic != null)
				{
					var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(statistic), Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PutAsync($"api/Statistics/{statistic.Id}", content);
				}



			}
		}
        public async Task GetValueFromStatisticAsync(int fromStorageId, int toStorageId, int productId, int quantity, string? notes)
        {

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.GetOneUserAsync(userId);
            var product = await _productManager.GetProductByIdAsync(productId, false);
            var initialStorage = await _storageManager.GetStorageByIdAsync(fromStorageId, false);
            var destinationStorage = await _storageManager.GetStorageByIdAsync(toStorageId, false);

			var newStatistic = new Statistic
			{
				UserId = userId,
				UserName = user?.UserName,
				EmployeeNumber = user?.EmployeeNumber,
				ProductId = productId,
				ProductName = product?.Name,
				Quantity = quantity,
				InitialStorageId = fromStorageId,
				IntitialStorageName = initialStorage?.Name,
				DestinationStorageId = toStorageId,
				DestinationStorageName = destinationStorage?.Name,
				Moved = DateTime.Now,
				Notes = notes != null ? notes : "Moved"
			};

            await CreateStatisticAsync(newStatistic);
        }
    }
}
