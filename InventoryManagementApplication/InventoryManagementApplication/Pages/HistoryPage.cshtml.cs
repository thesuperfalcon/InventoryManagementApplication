using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Pages
{
    public class HistoryPageModel : PageModel
    {
        private readonly InventoryManagementApplicationContext _context;

        public HistoryPageModel(InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        public List<Statistic> Statistics { get; set; } = new List<Statistic>();


        public async Task OnGetAsync()
        {
            Statistics = await _context.Statistics
                .Include(x => x.InitialStorage)
                .Include(x => x.DestinationStorage)
                .Include(x => x.Product)
                .Include(x => x.User)
                .ToListAsync();

            Statistics = Statistics.Where(statistic =>
            statistic?.Product != null &&
            statistic.DestinationStorage != null &&
            statistic.InitialStorage != null &&
            statistic.User != null).ToList();


        }

    }
}
