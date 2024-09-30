using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InventoryManagementApplication.Models;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Areas.Identity.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace InventoryManagementApplication.Pages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly InventoryManagementApplicationContext _context;

        public ProductDetailsModel(InventoryManagementApplicationContext context)
        {
            _context = context;
        }
        public Product Product { get; set; }

        //Sidor

    // Deklarera variabler för sidhantering
    public List<ActivityLog> PagedActivityLogs { get; set; } = new List<ActivityLog>();
    public List<InventoryTracker> PagedInventoryTrackers { get; set; } = new List<InventoryTracker>();

    public int PageNumber { get; set; } = 1;      // Aktuellt sidnummer
    public int TotalPages { get; set; }           // Totalt antal sidor
    public int PageSize { get; set; } = 5;        // Antal poster per sida

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public int ActivityPageNumber { get; set; } = 1;      // Aktuellt sidnummer för aktivitetsloggar
    public int ActivityTotalPages { get; set; }           // Totalt antal sidor för aktivitetsloggar
    public bool HasPreviousActivityPage => ActivityPageNumber > 1;
    public bool HasNextActivityPage => ActivityPageNumber < ActivityTotalPages;

    public int InventoryPageNumber { get; set; } = 1;     // Aktuellt sidnummer för lagerförändringar
    public int InventoryTotalPages { get; set; }          // Totalt antal sidor för lagerförändringar
    public bool HasPreviousInventoryPage => InventoryPageNumber > 1;
    public bool HasNextInventoryPage => InventoryPageNumber < InventoryTotalPages;

       
        public List<Storage> Storages { get; set; }
        public InventoryManagementUser SelectedUser { get; set; }
         public List<InventoryTracker> InventoryTrackers { get; set; }

        /* Commenting out the product movement related properties */
        // public int? CurrentStorageId { get; set; }


        public async Task<IActionResult> OnGetAsync(int id, int activityPageNumber = 1, int inventoryPageNumber = 1)
        {
            Product = await _context.Products
                .Include(p => p.InventoryTrackers)
                    .ThenInclude(it => it.Storage)
                .Include(p => p.ActivityLogs)
                    .ThenInclude(log => log.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            // Check if the product exists
            if (Product == null)
            {
                return NotFound();
            }


    // Paginering för aktivitetsloggar
    int totalLogs = Product.ActivityLogs.Count();
    ActivityTotalPages = (int)Math.Ceiling(totalLogs / (double)PageSize);
    ActivityPageNumber = activityPageNumber;
    PagedActivityLogs = Product.ActivityLogs
        .OrderByDescending(log => log.TimeStamp)
        .Skip((ActivityPageNumber - 1) * PageSize)
        .Take(PageSize)
        .ToList();

    // Paginering för lagerförändringar
    int totalTrackers = Product.InventoryTrackers.Count();
    InventoryTotalPages = (int)Math.Ceiling(totalTrackers / (double)PageSize);
    InventoryPageNumber = inventoryPageNumber;
    PagedInventoryTrackers = Product.InventoryTrackers
        .Skip((InventoryPageNumber - 1) * PageSize)
        .Take(PageSize)
        .ToList();

    return Page();

            /* Commenting out storage fetching logic */
            // Storages = await _context.Storages.ToListAsync();

            /* Commenting out logic to set CurrentStorageId */
            // var tracker = Product.InventoryTrackers.FirstOrDefault();
            // if (tracker != null)
            // {
            //     CurrentStorageId = tracker.StorageId;
            // }

        }

        public async Task<IActionResult> OnPostUpdateProductInfoAsync(int ProductId, string ProductName, string ProductDescription /*, int StorageId*/)
        {
            var product = await _context.Products
                .Include(p => p.InventoryTrackers)
                .FirstOrDefaultAsync(p => p.Id == ProductId);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = ProductName;
            product.Description = ProductDescription;

            /* Commenting out product movement related logic */
            // var tracker = product.InventoryTrackers.FirstOrDefault();
            // if (tracker != null)
            // {
            //     tracker.StorageId = StorageId;
            // }
            // else
            // {
            //     product.InventoryTrackers.Add(new InventoryTracker
            //     {
            //         ProductId = ProductId,
            //         StorageId = StorageId,
            //         Quantity = product.CurrentStock,
            //         Modified = DateTime.Now
            //     });
            // }

            

            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = ProductId });
        }
    }
}
