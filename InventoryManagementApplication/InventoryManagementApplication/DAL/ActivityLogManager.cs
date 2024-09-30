using InventoryManagementApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
    public class ActivityLogManager
    {
        private static readonly Uri BaseAddress = new Uri("https://localhost:44353/");
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActivityLog ActivityLog { get; set; }
        public List<ActivityLog> ActivityLogs { get; set; }

        public ActivityLogManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateActivityLogAsync(ActivityLog activityLog)
        {
            if (activityLog != null)
            {
                ActivityLog = activityLog;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = BaseAddress;
                    var json = JsonSerializer.Serialize(ActivityLog);

                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("api/ActivityLogs/", httpContent);
                }
            }
            else
            {
                Console.WriteLine("Error! Abort!");
            }
        }

        public async Task<List<ActivityLog>> GetAllActivityLogsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync("api/ActivityLogs");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    List<Models.ActivityLog> activityLogs = JsonSerializer.Deserialize<List<Models.ActivityLog>>(responseString);
                    ActivityLogs = activityLogs.ToList();
                }

                return ActivityLogs;
            }
        }

        public async Task DeleteActivityLogAsync(int? id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                var response = await client.DeleteAsync($"api/ActivityLogs/{id}");
            }
        }

        public async Task EditActivityLogAsync(ActivityLog? activityLog)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                if (activityLog != null)
                {
                    ActivityLog = activityLog;
                    var content = new StringContent(JsonSerializer.Serialize(ActivityLog), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync($"api/ActivityLogs/{ActivityLog.Id}", content);
                }
            }
        }

        public async Task LogActivityAsync(object entity, EntityState state)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var actionType = state == EntityState.Added ? ActionType.Created :
                             state == EntityState.Modified ? ActionType.Updated :
                             ActionType.Deleted;

            var log = new ActivityLog
            {
                UserId = userId,
                Action = actionType,
                ItemType = entity is Product ? ItemType.Product : ItemType.Storage,
                TypeId = entity is Product product ? product.Id : (entity is Storage storage ? storage.Id : null),
                TimeStamp = DateTime.UtcNow,
                Notes = "Optional notes about the action"
            };

            await CreateActivityLogAsync(log);
        }
    }
}
