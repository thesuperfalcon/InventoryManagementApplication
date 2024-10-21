using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
    public class UserManager
    {
        public InventoryManagementUser? User { get; set; }
        public List<InventoryManagementUser>? Users { get; set; }

		private static readonly Uri BaseAddress = new Uri("https://localhost:44353/");

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


        public async Task<List<InventoryManagementUser>> SearchUsersAsync(string? inputValue)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                string uri = "api/Users/SearchUsers";


                if (!string.IsNullOrEmpty(inputValue))
                {
                    uri += $"?inputValue={inputValue}";
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


        public async Task<bool> ResetPassword(InventoryManagementUser? user, List<string?>? currentRoles, string? addRoles, bool resetPassword)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = BaseAddress;

                var resetPasswordRequest = Helpers.DTOHelpers.SetDTO(user, currentRoles, addRoles, resetPassword);              
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(resetPasswordRequest), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"api/Users/{user.Id}", content);

                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> EditUserAsync(InventoryManagementUser? user, List<string?>? currentRoles, string? addRole, bool resetPassword)
        {


            using (var client = new HttpClient())
            {

                client.BaseAddress = BaseAddress;
                var editUserRequest = Helpers.DTOHelpers.SetDTO(user, currentRoles, addRole, resetPassword);
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

        public async Task<List<string>> GetPicUrlAsync()
        {
            List<string>? picUrls = new List<string>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                var response = await client.GetAsync($"api/ProfilePic/");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    picUrls = JsonSerializer.Deserialize<List<string>>(responseString);
                }
                return picUrls;
            }
        }

        public async Task<List<string>> GetUserRoleAsync(string userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                HttpResponseMessage response = await client.GetAsync($"api/Users/UserRole/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var roles = await response.Content.ReadFromJsonAsync<List<string>>();
                    return roles ?? new List<string>();
                }
                return new List<string>();
            }
        }

        public async Task<List<Developer>> GetAllDevelopersAsync()
        {
            var developers = new List<Developer>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                var response = await client.GetAsync($"api/Developers/");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    developers = JsonSerializer.Deserialize<List<Developer>>(responseString);
                }
                return developers;
            }

        }
    }
}