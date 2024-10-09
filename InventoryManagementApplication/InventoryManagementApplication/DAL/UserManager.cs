using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.Models;
using System.Text;
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

        public async Task<List<InventoryManagementUser>> GetAllUsersAsync(bool? isDeleted)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = BaseAddress;
                
                string uri = "api/Users/";
                if (isDeleted != null)
                {
                    uri += isDeleted == false ? "ExistingUsers" : "DeletedUsers";
                }
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseString);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<InventoryManagementUser> users = JsonSerializer.Deserialize<List<InventoryManagementUser>>(responseString);
                    Users = users.ToList();
                }


                return Users;
            }
        }
        public async Task<InventoryManagementUser> GetOneUserAsync(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                HttpResponseMessage response = await client.GetAsync($"api/Users/{id}");

                if (response.IsSuccessStatusCode)
                {
                    InventoryManagementUser user = await response.Content.ReadFromJsonAsync<InventoryManagementUser>();

                    if (user != null)
                    {
                        return user; 
                    }
                }

                return null;
            }
        }


        public async Task<List<InventoryManagementUser>> SearchUsersAsync(string? name, string? employeeNumber)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                string uri = "api/Users/SearchUsers";


                if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(employeeNumber))
                {
                    uri += $"?name={Uri.EscapeDataString(name ?? "")}&employeeNumber={Uri.EscapeDataString(employeeNumber ?? "")}";
                }

                HttpResponseMessage responseUsers = await client.GetAsync(uri);

                List<InventoryManagementUser> users = new List<InventoryManagementUser>();

                if (responseUsers.IsSuccessStatusCode)
                {
                    string responseString = await responseUsers.Content.ReadAsStringAsync();
                    users = JsonSerializer.Deserialize<List<InventoryManagementUser>>(responseString) ?? new List<InventoryManagementUser>();
                }

                return users;
            }
        }


        public async Task<bool> ResetPassword(InventoryManagementUser? user, List<string?>? currentRoles)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = BaseAddress;
                var resetPasswordRequest = new DTO.RolesDTO
                {
                    User = user,
                    CurrentRoles = currentRoles,
                    AddRole = null,
                    ResetPassword = true
                };
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(resetPasswordRequest), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"api/Users/{user.Id}", content);

                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> EditUserAsync(InventoryManagementUser? user, List<string?>? currentRoles)
        {

   
            using (var client = new HttpClient())
            {

                client.BaseAddress = BaseAddress;
                var editUserRequest = new DTO.RolesDTO
                {
                    User = user,
                    CurrentRoles = null,
                    AddRole = null,
                    ResetPassword = false
                };
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(editUserRequest), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"api/Users/{user.Id}", content);

                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> DeleteUserAsync(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                var response = await client.DeleteAsync($"api/Users/{id}");

                return response.IsSuccessStatusCode;
            }
        }
    }
}