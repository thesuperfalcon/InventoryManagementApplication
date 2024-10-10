using InventoryManagementApplication.Models;
using InventoryManagementApplication.DAL;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using InventoryManagementApplication.Data;


namespace InventoryManagementApplication.Pages
{
public class ProductDetailsModel : PageModel
    {
        private readonly LogManager _logManager;
        private readonly UserManager _userManager;

     private readonly ProductManager _productManager;

        public ProductDetailsModel(ProductManager productManager,  LogManager logManager, UserManager userManager)
        {
            _productManager = productManager;
          
            _logManager = logManager;
            _userManager = userManager;
        }

        public Product Product { get; set; }
        public List<Log> ActivityLogs { get; set; } = new List<Log>();
        public List<string> UserFullName { get; set; } = new List<string>();
        public List<string> UserEmployeeNumbers { get; set; } = new List<string>();

      public async Task<IActionResult> OnGetAsync(int id)
{
    // Hämta produkten via API:et
    Product = await _productManager.GetProductByIdAsync(id, null);

    if (Product == null)
    {
        return NotFound();
    }

    // Hämta alla loggar och filtrera efter produkten
    var allLogs = await _logManager.GetAllLogsAsync();
    ActivityLogs = allLogs.Where(log => log.EntityType.Contains("Product") && log.EntityName == Product.Name).ToList();

    // Hämta användarinformation för varje logg
    var users = await _userManager.GetAllUsersAsync(false);

    var userDictionary = users.ToDictionary(
        u => u.Id,
        u => new { FullName = $"{u.LastName} {u.FirstName}", EmployeeNumber = u.EmployeeNumber });

    foreach (var log in ActivityLogs)
    {
        if (userDictionary.TryGetValue(log.UserId, out var userInfo))
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

    return Page();
}
    
public async Task<IActionResult> OnPostUpdateProductInfoAsync(int ProductId, string ProductName, string ProductDescription)
{
    // Hämta produkten via API
    var product = await _productManager.GetProductByIdAsync(ProductId, null);

    if (product == null)
    {
        return NotFound();
    }

    product.Name = ProductName;
    product.Description = ProductDescription;

    // Spara ändringarna via API:et
    await _productManager.EditProductAsync(product);

    return RedirectToPage(new { id = ProductId });
}
    }
}
