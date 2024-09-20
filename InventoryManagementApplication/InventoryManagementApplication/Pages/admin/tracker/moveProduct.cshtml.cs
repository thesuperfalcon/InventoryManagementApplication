using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Helpers;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InventoryManagementApplication.Pages.admin.tracker
{
    public class moveProductModel : PageModel
    {
        private readonly InventoryManagementApplicationContext _context;
        private readonly UserManager<InventoryManagementUser> _userManager;
        private readonly SelectListHelpers _selectListHelpers;

        private static Uri BaseAddress = new Uri("https://localhost:44353/");
        public moveProductModel(InventoryManagementApplicationContext context, UserManager<InventoryManagementUser> userManager, SelectListHelpers selectListHelpers)
        {
            _context = context;
            _userManager = userManager;
            _selectListHelpers = selectListHelpers;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public SelectList StorageSelectList { get; set; }
        public SelectList ProductSelectList { get; set; }
        [BindProperty]
        public InventoryTracker InventoryTracker { get; set; } = default!;
        public Storage Storage { get; set; }
        public Product Product { get; set; }
        public InventoryTracker MovingInventoryTracker { get; set; }
        [BindProperty]
        public InventoryTracker SelectedInventoryTracker { get; set; }
        public InventoryManagementUser MyUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            MyUser = await _userManager.GetUserAsync(User);
            SelectedInventoryTracker = await _context.InventoryTracker.Include(x => x.Product).Include(z => z.Storage).Where(g => g.Id == id).FirstOrDefaultAsync();


            if (SelectedInventoryTracker == null)
            {
                return RedirectToPage("./Index");
            }

            StorageSelectList = await _selectListHelpers.GenerateStorageSelectListAsync(SelectedInventoryTracker.StorageId);
            ProductSelectList = await _selectListHelpers.GenerateProductSelectListAsync();

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            MyUser = await _userManager.GetUserAsync(User);

            var currentTracker = await _context.InventoryTracker
                .Where(x => x.ProductId == SelectedInventoryTracker.ProductId && x.StorageId == SelectedInventoryTracker.StorageId)
                .FirstOrDefaultAsync();

            if (currentTracker == null)
            {
                return Page();
            }

            if (currentTracker.Quantity < InventoryTracker.Quantity)
            {
                return Page();
            }

            currentTracker.Quantity -= (int)InventoryTracker.Quantity;

            var destinationTracker = await _context.InventoryTracker
                .Where(x => x.ProductId == SelectedInventoryTracker.ProductId && x.StorageId == InventoryTracker.StorageId)
                .FirstOrDefaultAsync();

            if (destinationTracker == null)
            {
                destinationTracker = new InventoryTracker
                {
                    ProductId = SelectedInventoryTracker.ProductId,
                    StorageId = InventoryTracker.StorageId,
                    Quantity = (int)InventoryTracker.Quantity
                };
                _context.InventoryTracker.Add(destinationTracker);
            }
            else
            {
                destinationTracker.Quantity += (int)InventoryTracker.Quantity;
            }

            var sourceStorage = await _context.Storages.FindAsync(SelectedInventoryTracker.StorageId);
            var destinationStorage = await _context.Storages.FindAsync(InventoryTracker.StorageId);
            var product = await _context.Products.FindAsync(SelectedInventoryTracker.ProductId);

            if (sourceStorage == null || destinationStorage == null)
            {
                return Page();
            }

            sourceStorage.CurrentStock -= (int)InventoryTracker.Quantity;
            destinationStorage.CurrentStock += (int)InventoryTracker.Quantity;

            sourceStorage.Updated = DateTime.Now;
            destinationStorage.Updated = DateTime.Now;

            product.Updated = DateTime.Now;

            currentTracker.Modified = DateTime.Now;
            destinationTracker.Modified = DateTime.Now;










            var statistic = new Statistic
            {
                UserId = MyUser?.Id,
                InitialStorageId = SelectedInventoryTracker.StorageId,
                DestinationStorageId = InventoryTracker.StorageId,
                ProductId = SelectedInventoryTracker.ProductId,
                ProductQuantity = (int)InventoryTracker.Quantity,
                OrderTime = DateTime.Now,
                Completed = false
            };


            await SaveStatisticsAsync(statistic);

            return RedirectToPage("./Index");
        }

        public static async Task SaveStatisticsAsync(Statistic statistic)
        {

            var statistics = (await StatisticPageModel.GetStatisticsAsync()).Where(c => c.Id == statistic.Id).SingleOrDefault();

            if (statistics != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = BaseAddress;

                    var json = JsonSerializer.Serialize(statistics);

                    StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage responseMessage = await client.PutAsync("api/Statictics/" + statistic.Id, httpContent);

                }
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = BaseAddress;

                    var json = JsonSerializer.Serialize(statistic);

                    StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage responseMessage = await client.PostAsync("api/Statistics", httpContent);

                }
            }
        }
    }
}