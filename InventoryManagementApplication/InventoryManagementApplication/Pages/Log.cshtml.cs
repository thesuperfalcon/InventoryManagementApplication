using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementApplication.Pages
{
    public class LogModel : PageModel
    {
        private readonly LogManager _activityLogManager;
        private readonly UserManager _userManager;

        // Se till att denna egenskap är korrekt definierad
        public List<Log> Logs { get; set; } = new List<Log>();
        public List<string> UserFullName { get; set; } = new List<string>();
        public List<string> UserEmployeeNumbers { get; set; } = new List<string>();

        public LogModel(LogManager activityLogManager, UserManager userManager)
        {
            _activityLogManager = activityLogManager;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            // Hämta loggar från ActivityLogManager
            Logs = await _activityLogManager.GetAllLogsAsync();

            // Hämtar users och sätter ihop firstName + lastName till UserFullName
            var users = await _userManager.GetAllUsersAsync(null);
            var userDictinary = users.ToDictionary(
                u => u.Id,
                u => new { FullName = $"{u.LastName} {u.FirstName}", EmployeeNumber = u.EmployeeNumber });


            foreach (var log in Logs)
            {
                if(userDictinary.TryGetValue(log.UserId, out var userInfo))
                {
                    UserFullName.Add(userInfo.FullName);
                    UserEmployeeNumbers.Add(userInfo.EmployeeNumber);
                }
                else
                {
                    UserFullName.Add("Unknown User");
                    UserEmployeeNumbers.Add("N/A");
                }
            }


        }
    }
}
