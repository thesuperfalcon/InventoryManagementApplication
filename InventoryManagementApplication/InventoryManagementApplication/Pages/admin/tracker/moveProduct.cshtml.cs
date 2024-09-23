using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.DAL;
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

        private static Uri BaseAddress = new Uri("https://localhost:44353/");
        private readonly InventoryManagementApplicationContext _context;
        private readonly UserManager<InventoryManagementUser> _userManager;
        private readonly SelectListHelpers _selectListHelpers;
        private readonly TrackerManager _trackerManager;
        private readonly StorageManager _storageManager;
        private readonly ProductManager _productManager;

        public moveProductModel(InventoryManagementApplicationContext context, UserManager<InventoryManagementUser> userManager, 
            SelectListHelpers selectListHelpers, TrackerManager trackerManager, StorageManager storageManager, ProductManager productManager)
        {
            _context = context;
            _userManager = userManager;
            _selectListHelpers = selectListHelpers;
            _trackerManager = trackerManager;
            _storageManager = storageManager;
            _productManager = productManager;
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
            //�ndra user?
            MyUser = await _userManager.GetUserAsync(User);
			var trackerSelect = await _trackerManager.GetAllTrackersAsync();

            SelectedInventoryTracker = trackerSelect.Where(tr => tr.Id == id).FirstOrDefault();
            if(SelectedInventoryTracker != null)
            {
                SelectedInventoryTracker.ProductId = SelectedInventoryTracker.Product.Id;
                SelectedInventoryTracker.StorageId = SelectedInventoryTracker.Storage.Id;

			}
			//SelectedInventoryTracker = await _context.InventoryTracker.Include(x => x.Product).Include(z => z.Storage).Where(g => g.Id == id).FirstOrDefaultAsync();


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
            var storageList = await _storageManager.GetAllStoragesAsync();
            var trackerList = await _trackerManager.GetAllTrackersAsync();
            var productList = await _productManager.GetAllProductsAsync();

            var currentTracker = trackerList.Where(x => x.Product.Id == SelectedInventoryTracker.ProductId && x.Storage.Id == SelectedInventoryTracker.StorageId)
				.FirstOrDefault();
			//var currentTracker = await _context.InventoryTracker
                //.Where(x => x.ProductId == SelectedInventoryTracker.ProductId && x.StorageId == SelectedInventoryTracker.StorageId)
               // .FirstOrDefaultAsync();

            if (currentTracker == null)
            {
                return Page();
            }

            if (currentTracker.Quantity < InventoryTracker.Quantity)
            {
                return Page();
            }

            currentTracker.Quantity -= (int)InventoryTracker.Quantity;

            var destinationTracker = trackerList.Where(x => x.Product.Id == SelectedInventoryTracker.ProductId && x.Storage.Id == InventoryTracker.StorageId)
                .FirstOrDefault();
			//var destinationTracker = await _context.InventoryTracker
			//    .Where(x => x.ProductId == SelectedInventoryTracker.ProductId && x.StorageId == InventoryTracker.StorageId)
			//    .FirstOrDefaultAsync();

			if (destinationTracker == null)
            {
                destinationTracker = new InventoryTracker
                {
                    ProductId = SelectedInventoryTracker.ProductId,
                    StorageId = InventoryTracker.StorageId,
                    Quantity = (int)InventoryTracker.Quantity
                };
                await _trackerManager.CreateTrackerAsync(destinationTracker);
               // _context.InventoryTracker.Add(destinationTracker);
            }
            else
            {
                destinationTracker.Quantity += (int)InventoryTracker.Quantity;
            }
            var sourceStorage = storageList.Find(s => s.Id == SelectedInventoryTracker.StorageId);
            //var sourceStorage = await _context.Storages.FindAsync(SelectedInventoryTracker.StorageId);
           
            var destinationStorage = storageList.Find(s => s.Id == InventoryTracker.StorageId);

            var destinationStorageTest = await _storageManager.GetOneStorageAsync(InventoryTracker.StorageId);
            //var destinationStorage = await _context.Storages.FindAsync(InventoryTracker.StorageId);
            
            
            var product = await _productManager.GetOneProductAsync(SelectedInventoryTracker.ProductId); 
            //var product = await _context.Products.FindAsync(SelectedInventoryTracker.ProductId);

            if (sourceStorage == null || destinationStorage == null)
            {
                return Page();
            }

            sourceStorage.CurrentStock -= (int)InventoryTracker.Quantity;
            destinationStorage.CurrentStock += (int)InventoryTracker.Quantity;

            sourceStorage.Updated = DateTime.Now;
            destinationStorage.Updated = DateTime.Now;
            await _storageManager.EditStorageAsync(sourceStorage);
            await _storageManager.EditStorageAsync(destinationStorage);

            product.Updated = DateTime.Now;

            await _productManager.EditProductAsync(product);

            currentTracker.Modified = DateTime.Now;
            destinationTracker.Modified = DateTime.Now;



            var activityLog = new ActivityLog
            {
                UserId = MyUser?.Id,
                ItemType = (ItemType?)0,
                Action = (ActionType?)3,
                TypeId = SelectedInventoryTracker.ProductId,
                TimeStamp = DateTime.Now,
                Notes = "",
                
            };






            var statistic = new Statistic
            {
                UserId = MyUser?.Id,
                InitialStorageId = SelectedInventoryTracker.StorageId,
                DestinationStorageId = InventoryTracker.StorageId,
                ProductId = SelectedInventoryTracker.ProductId,
                ProductQuantity = (int)InventoryTracker.Quantity,
                OrderTime = DateTime.Now,
                Completed = false,

                // �ndra _context till DAL metod.
                // DestinationStorage = await _context.Storages.FindAsync(InventoryTracker.StorageId),
                // InitialStorage = await _context.Storages.FindAsync(SelectedInventoryTracker.StorageId)
                // Product = await _context.Products.FindAsync(SelectedInventoryTracker.ProductId)
            };

            await SaveActivityLogAsync(activityLog);
            await SaveStatisticsAsync(statistic);

            return RedirectToPage("./Index");
        }


        // L�gg in denna metod i DAL mapp
        public static async Task SaveStatisticsAsync(Statistic statistic)
        {

            var statistics = (await StatisticModel.GetStatisticsAsync()).Where(c => c.Id == statistic.Id).SingleOrDefault();

            if (statistics != null && statistics.Id > 0)
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

        public static async Task SaveActivityLogAsync(ActivityLog activityLog)
        {

            var activityLogs = (await LogModel.GetActivityLogAsync()).Where(c => c.Id == activityLog.Id).FirstOrDefault();

            if (activityLogs != null && activityLogs.Id > 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = BaseAddress;

                    var json = JsonSerializer.Serialize(activityLog);

                    StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage responseMessage = await client.PutAsync("api/ActivityLogs/" + activityLogs.Id, httpContent);

                }
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = BaseAddress;

                    var json = JsonSerializer.Serialize(activityLog);

                    StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage responseMessage = await client.PostAsync("api/ActivityLogs", httpContent);

                }
            }
        }
    }
}