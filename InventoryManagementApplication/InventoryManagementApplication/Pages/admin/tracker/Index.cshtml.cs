using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using InventoryManagementApplication.DAL;

namespace InventoryManagementApplication.Pages.admin.tracker
{
    public class IndexModel : PageModel
    {
        private readonly TrackerManager _trackerManager;

        public IndexModel(TrackerManager trackerManager)
        {
            _trackerManager = trackerManager;
        }

        public IList<InventoryTracker> InventoryTracker { get;set; } = default!;

        public async Task OnGetAsync()
        {
            InventoryTracker = await _trackerManager.GetAllTrackersAsync(false);
        }
    }
}
