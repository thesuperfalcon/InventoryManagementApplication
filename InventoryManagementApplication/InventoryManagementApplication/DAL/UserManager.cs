using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.Models;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
	public class UserManager
	{
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public Areas.Identity.Data.InventoryManagementUser? User { get; set; }
		public List<Areas.Identity.Data.InventoryManagementUser>? Users { get; set; }


		public async Task<HttpResponseMessage> RegisterUserAsync(InventoryManagementUser user)
		{
			HttpResponseMessage response = null;
			if (user != null)
			{
				User = user;
				using (var client = new HttpClient())
				{
					client.BaseAddress = BaseAddress;
					var json = JsonSerializer.Serialize(User);

					StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
					response = await client.PostAsync("api/Users/", httpContent);
				}
			}
			else
			{
				Console.WriteLine("Error! Abort!");
			}
			return response;
		}

		public async Task<List<InventoryManagementUser>> GetAllUsersAsync()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage response = await client.GetAsync("api/Users/");

				if (response.IsSuccessStatusCode)
				{
					string responseString = await response.Content.ReadAsStringAsync();
					List<InventoryManagementUser> users = JsonSerializer.Deserialize<List<InventoryManagementUser>>(responseString);
					Users = users.ToList();
				}

				return Users;
			}
		}
		public async Task<InventoryManagementUser> GetOneUserAsync(int id)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage response = await client.GetAsync($"api/Users/{id}");

				if (response.IsSuccessStatusCode)
				{
					string responseString = await response.Content.ReadAsStringAsync();
					InventoryManagementUser user = JsonSerializer.Deserialize<InventoryManagementUser>(responseString);
					User = user;
				}

				return User;
			}
		}
	}
}
