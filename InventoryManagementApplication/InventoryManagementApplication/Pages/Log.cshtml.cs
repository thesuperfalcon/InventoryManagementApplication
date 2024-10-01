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

        // Se till att denna egenskap �r korrekt definierad
        public List<Log> Logs { get; set; } = new List<Log>();

        public LogModel(LogManager activityLogManager)
        {
            _activityLogManager = activityLogManager;
        }

        public async Task OnGetAsync()
        {
            // H�mta loggar fr�n ActivityLogManager
            Logs = await _activityLogManager.GetAllLogsAsync();
        }
    }
}
