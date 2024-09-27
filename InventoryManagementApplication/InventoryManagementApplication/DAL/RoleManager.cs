using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
    public class RoleManager
    {
        private static Uri BaseAddress = new Uri("https://localhost:44353/");

        public List<Areas.Identity.Data.InventoryManagementRole>? Roles { get; set; }




        public async Task<List<InventoryManagementRole>> GetAllRoles()
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync("api/Roles/");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseString);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<InventoryManagementRole> roles = JsonSerializer.Deserialize<List<InventoryManagementRole>>(responseString);
                    Roles = roles.ToList();
                }


                return Roles;
            }
        }
    }
}
