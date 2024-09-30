using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.DTO;
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
        private readonly StatisticManager _statisticManager;

        public moveProductModel(InventoryManagementApplicationContext context, UserManager<InventoryManagementUser> userManager, 
            SelectListHelpers selectListHelpers, TrackerManager trackerManager,
            StorageManager storageManager, ProductManager productManager, ProductMovementHelpers productMovementHelpers, 
            StatisticManager statisticManager)
        {
            _context = context;
            _userManager = userManager;
            _selectListHelpers = selectListHelpers;
            _trackerManager = trackerManager;
            _storageManager = storageManager;
            _productManager = productManager;
            _productMovementHelpers = productMovementHelpers;
            _statisticManager = statisticManager;
        }

        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InventoryTracker InventoryTracker { get; set; } = default!;
        [BindProperty]
        public InventoryTracker SelectedInventoryTracker { get; set; }
        public SelectList StorageSelectList { get; set; }
        public SelectList ProductSelectList { get; set; }
        public InventoryManagementUser MyUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            MyUser = await _userManager.GetUserAsync(User);

            SelectedInventoryTracker = await _trackerManager.GetOneTrackerAsync(id.Value);

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
            MyUser = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            int productId = (int)SelectedInventoryTracker.ProductId;
            int fromStorageId = (int)SelectedInventoryTracker.StorageId;
            int toStorageId = (int)InventoryTracker.StorageId;
            int quantity = (int)InventoryTracker.Quantity;

            string errorMessage = null;

            switch (true)
            {
                case bool when productId < 0:
                    errorMessage = "Ogiltigt produkt-ID. Vänligen ange ett giltigt produkt-ID.";
                    break;
                case bool when fromStorageId < 0:
                    errorMessage = "Ogiltigt från-lager-ID. Vänligen ange ett giltigt från-lager.";
                    break;
                case bool when toStorageId < 0:
                    errorMessage = "Ogiltigt till-lager-ID. Vänligen ange ett giltigt till-lager.";
                    break;
                case bool when quantity < 0:
                    errorMessage = "Ogiltig kvantitet. Vänligen ange en positiv kvantitet.";
                    break;
            }

            if (errorMessage != null)
            {
                StatusMessage = errorMessage;
                return RedirectToPage("./moveProduct", new { id = SelectedInventoryTracker.Id });
            }


            bool status = false;

            var tuple = await _productMovementHelpers.MoveProductAsync(productId, fromStorageId, toStorageId, quantity);
            if (tuple != null)
            {
                status = tuple.Item1;
                if (status == false)
                {
                    StatusMessage = tuple.Item2 != string.Empty ? tuple.Item2 : "Förflyttning lyckades ej!";
                    return RedirectToPage("./moveProduct", new { id = SelectedInventoryTracker.Id });
                }
                else
                {
                    StatusMessage = tuple.Item2 != string.Empty ? tuple.Item2 : "Förflyttning lyckades!";
                   
                    await _statisticManager.CreateStatisticAsync(MyUser.Id, fromStorageId, toStorageId, productId, quantity);

                    return RedirectToPage("./moveProduct", new { id = SelectedInventoryTracker.Id });
                }
            }
            StatusMessage = "Förflyttning lyckades ej";
               
            return RedirectToPage("./moveProduct", new { id = SelectedInventoryTracker.Id });          
        }

    }
}