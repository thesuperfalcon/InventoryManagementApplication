using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Text.Json;

namespace InventoryManagementApplication.Pages
{
    public class LogModel : PageModel
    {
        private static Uri BaseAddress = new Uri("https://localhost:44353/");

        private readonly ActivityLogManager _activityLogManager;

        public LogModel(ActivityLogManager activityLogManager)
        {
            _activityLogManager = activityLogManager;
        }

        public List<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();




        public async void OnGet()
        {
            ActivityLogs = await _activityLogManager.GetAllActivityLogssAsync();
            //ActivityLogs = await GetActivityLogAsync();

        }


        public static async Task<List<ActivityLog>> GetActivityLogAsync()
        {
            var activityLogs = new List<ActivityLog>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                HttpResponseMessage responseMessage = await client.GetAsync("api/ActivityLogs/");
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = await responseMessage.Content.ReadAsStringAsync();
                    activityLogs = JsonSerializer.Deserialize<List<ActivityLog>>(responseString);
                }
                return activityLogs;
            }
        }


       
    }
}
