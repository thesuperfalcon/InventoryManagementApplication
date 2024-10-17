using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
    public class RoleManager
    {
        private static Uri BaseAddress = new Uri("https://localhost:44353/");

        public List<Areas.Identity.Data.InventoryManagementRole>? Roles { get; set; }

        public async Task<List<InventoryManagementRole>> GetAllRolesAsync()
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

        //public async Task RemoveFromRoleAsync(InventoryManagementUser? user, List<string?>? currentRoles)
        //{
        //    using (var client = new HttpClient())
        //    {


        //        client.BaseAddress = BaseAddress;
        //        var removeRoleRequest = new DTO.RolesDTO
        //        {
        //            User = user,
        //            CurrentRoles = currentRoles,
        //            AddRole = null,
        //            ResetPassword = false
        //        };
        //        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(removeRoleRequest), Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await client.PutAsync($"api/Users/{user.Id}", content);


        //    }
        //}

        //public async Task<bool> AddToRoleAsync(InventoryManagementUser? user, string? currentRoles)
        //{
        //    using (var client = new HttpClient())
        //    {

        //        client.BaseAddress = BaseAddress;
        //        var addRoleRequest = new DTO.RolesDTO
        //        {
        //            User = user,
        //            CurrentRoles = null,
        //            AddRole = currentRoles,
        //            ResetPassword = false
        //        };
        //        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(addRoleRequest), Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await client.PutAsync($"api/Users/{user.Id}", content);

        //        return response.IsSuccessStatusCode;
        //    }
        //}

        
    }
}
