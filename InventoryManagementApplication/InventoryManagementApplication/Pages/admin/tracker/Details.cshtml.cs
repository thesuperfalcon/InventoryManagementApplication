using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using System.Runtime.CompilerServices;
using InventoryManagementApplication.DAL;

namespace InventoryManagementApplication.Pages.admin.tracker
{
    public class DetailsModel : PageModel
    {
        private readonly TrackerManager _trackerManager;

        public DetailsModel(TrackerManager trackerManager)
        {
            _trackerManager = trackerManager;
        }

        public InventoryTracker InventoryTracker { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventorytracker = await _trackerManager.GetOneTrackerAsync((int)id);
            if (inventorytracker == null)
            {
                return NotFound();
            }
            else
            {
                InventoryTracker = inventorytracker;
            }
            return Page();
        }
    }
}
