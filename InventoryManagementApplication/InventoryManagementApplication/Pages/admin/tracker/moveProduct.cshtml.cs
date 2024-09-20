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

namespace InventoryManagementApplication.Pages.admin.tracker
{
    public class moveProductModel : PageModel
    {
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
            //Ändra user?
            MyUser = await _userManager.GetUserAsync(User);
			var trackerSelect = await _trackerManager.GetAllTrackersAsync();

            SelectedInventoryTracker = trackerSelect.Where(tr => tr.Id == id).FirstOrDefault();

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

            var currentTracker = trackerList.Where(x => x.ProductId == SelectedInventoryTracker.ProductId && x.StorageId == SelectedInventoryTracker.StorageId)
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

            await _trackerManager.EditTrackerAsync(currentTracker);
            await _trackerManager.EditTrackerAsync(destinationTracker);

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
            _context.Statistics.Add(statistic);

            //await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}