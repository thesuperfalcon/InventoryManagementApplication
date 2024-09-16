using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
	public class UserManager
	{
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public static async Task<List<Areas.Identity.Data.InventoryManagementUser>> GetAllUsersAsync()
		{

			List<Areas.Identity.Data.InventoryManagementUser> users = new List<Areas.Identity.Data.InventoryManagementUser>();

			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				HttpResponseMessage response = await client.GetAsync("/api/Users");
				if (response.IsSuccessStatusCode)
				{
					string responseString = await response.Content.ReadAsStringAsync();
					users = JsonSerializer.Deserialize<List<Areas.Identity.Data.InventoryManagementUser>>(responseString);
				}

				return users;
			}

		}
	}
}
