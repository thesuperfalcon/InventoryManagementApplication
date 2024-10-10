using InventoryManagementApplication.Models;
using InventoryManagementApplication.DAL;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using InventoryManagementApplication.Data;


namespace InventoryManagementApplication.Pages
{
    public class StorageDetailsModel : PageModel
    {
        private readonly InventoryManagementApplicationContext _context;
        private readonly LogManager _logManager;
        private readonly UserManager _userManager;

        public StorageDetailsModel(InventoryManagementApplicationContext context, LogManager logManager, UserManager userManager)
        {
            _context = context;
            _logManager = logManager;
            _userManager = userManager;
        }

        public Storage Storage { get; set; }
        public List<Log> ActivityLogs { get; set; } = new List<Log>();
        public List<string> UserFullName { get; set; } = new List<string>();
        public List<string> UserEmployeeNumbers { get; set; } = new List<string>();
        public ICollection<InventoryTracker> InventoryTrackers { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Hämtar lagret
            Storage = await _context.Storages
                .Include(s => s.InventoryTrackers)
                .ThenInclude(it => it.Product)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (Storage == null)
            {
                return NotFound();
            }

            var allLogs = await _logManager.GetAllLogsAsync();
            ActivityLogs = allLogs.Where(log => log.EntityType.Contains("Storage") && log.EntityName == Storage.Name).ToList();

            
            // Hämta användarinformation för varje logg
           //Denna hämtar icke-raderade användare
            var users = await _userManager.GetAllUsersAsync(false);
            var userDictinary = users.ToDictionary(
                u => u.Id,
                u => new { FullName = $"{u.LastName} {u.FirstName}", EmployeeNumber = u.EmployeeNumber });

            foreach (var log in ActivityLogs)
            {
                if (userDictinary.TryGetValue(log.UserId, out var userInfo))
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

            InventoryTrackers = Storage.InventoryTrackers;
            return Page();
        }

        //Uppdaterar lagerinformation
        public async Task<IActionResult> OnPostUpdateStorageAsync(int StorageId, string StorageName)
        {
            var storage = await _context.Storages.FindAsync(StorageId);
            if (storage == null) return NotFound();

            storage.Name = StorageName;
            storage.Updated = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = StorageId });
        }
    }
}