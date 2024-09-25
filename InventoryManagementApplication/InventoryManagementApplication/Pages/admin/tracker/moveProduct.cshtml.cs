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
        private readonly ProductMovementHelpers _productMovementHelpers;

        public moveProductModel(InventoryManagementApplicationContext context, UserManager<InventoryManagementUser> userManager, 
            SelectListHelpers selectListHelpers, TrackerManager trackerManager, StorageManager storageManager, ProductManager productManager, ProductMovementHelpers productMovementHelpers)
        {
            _context = context;
            _userManager = userManager;
            _selectListHelpers = selectListHelpers;
            _trackerManager = trackerManager;
            _storageManager = storageManager;
            _productManager = productManager;
            _productMovementHelpers = productMovementHelpers;
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

            int productId = (int)SelectedInventoryTracker.ProductId;
            int fromStorageId = (int)SelectedInventoryTracker.StorageId;
            int toStorageId = (int)InventoryTracker.StorageId;
            int quantity = (int)InventoryTracker.Quantity;

            if(productId < 0 ||fromStorageId < 0 || toStorageId < 0 || quantity < 0)
            {
                return RedirectToPage("./moveProduct", new { id = SelectedInventoryTracker.Id });
            }

            bool status = false;

            var tuple = await _productMovementHelpers.MoveProductAsync(productId, fromStorageId, toStorageId, quantity);
            if (tuple != null)
            {
                status = tuple.Item1;
                if (status == false)
                {
                    StatusMessage = "Förflyttning lyckades ej";
                    return RedirectToPage("./moveProduct", new { id = SelectedInventoryTracker.Id });
                }
                else
                {
                    StatusMessage = tuple.Item2;
                    return RedirectToPage("./Index");
                }
            }
            StatusMessage = "Förflyttning lyckades ej";
            return RedirectToPage("./moveProduct", new { id = SelectedInventoryTracker.Id });
            //MyUser = await _userManager.GetUserAsync(User);
            //var storageList = await _storageManager.GetStoragesAsync(false);
            //var trackerList = await _trackerManager.GetAllTrackersAsync();

            //var currentTracker = trackerList.FirstOrDefault(x => x.ProductId == SelectedInventoryTracker.ProductId && x.StorageId == SelectedInventoryTracker.StorageId);
            //if (currentTracker == null)
            //{
            //    return Page();
            //}

            //var destinationTracker = trackerList.FirstOrDefault(x => x.ProductId == SelectedInventoryTracker.ProductId && x.StorageId == InventoryTracker.StorageId);

            //// Skapa destinationTracker om den inte finns
            //if (destinationTracker == null)
            //{
            //    destinationTracker = new InventoryTracker
            //    {
            //        ProductId = SelectedInventoryTracker.ProductId,
            //        StorageId = InventoryTracker.StorageId,
            //        Quantity = (int)InventoryTracker.Quantity
            //    };
            //    await _trackerManager.CreateTrackerAsync(destinationTracker);
            //}
            //else
            //{
            //    destinationTracker.Quantity += (int)InventoryTracker.Quantity;
            //    await _trackerManager.EditTrackerAsync(destinationTracker);
            //}

            //if (currentTracker.Quantity < InventoryTracker.Quantity)
            //{
            //    StatusMessage = "Finns ej den m�ngden produkter i lagret. F�rs�k med en mindre m�ngd.";
            //    return RedirectToPage("./moveProduct", new { id = SelectedInventoryTracker.Id });
            //}

            //currentTracker.Quantity -= (int)InventoryTracker.Quantity;
            //await _trackerManager.EditTrackerAsync(currentTracker);

            //var sourceStorage = storageList.FirstOrDefault(s => s.Id == SelectedInventoryTracker.StorageId);
            //var destinationStorage = storageList.FirstOrDefault(s => s.Id == InventoryTracker.StorageId);

            //if (sourceStorage == null || destinationStorage == null)
            //{
            //    return Page();
            //}

            //// Uppdatera lagret
            //sourceStorage.CurrentStock -= (int)InventoryTracker.Quantity;
            //destinationStorage.CurrentStock += (int)InventoryTracker.Quantity;

            //await _storageManager.EditStorageAsync(sourceStorage);
            //await _storageManager.EditStorageAsync(destinationStorage);

            //// Spara uppdaterad produkt
            //var product = await _productManager.GetProductByIdAsync(SelectedInventoryTracker.ProductId, null);
            //product.Updated = DateTime.Now;
            //await _productManager.EditProductAsync(product);

            //return RedirectToPage("./Index");
        }

    }
}