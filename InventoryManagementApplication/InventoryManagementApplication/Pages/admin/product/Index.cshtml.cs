using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static InventoryManagementApplication.Pages.UsersRolesModel;

namespace InventoryManagementApplication.Pages.admin.product
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ProductManager _productManager;

        public IndexModel(ProductManager productManager)
        {
            _productManager = productManager;
        }
        public IList<Product> Products { get; set; } = default!;
        [BindProperty]
        public bool IsDeletedToggle { get; set; }
        public int ProductCount { get; set; }

        public async Task OnGet(bool isDeletedToggle = false)
        {
            IsDeletedToggle = isDeletedToggle;
            Products = await LoadProducts(IsDeletedToggle);
            ProductCount = Products.Count();
        }

        public async Task<IActionResult> OnPostAsync(int buttonId)
        {
            if (buttonId == 1)
            {
                IsDeletedToggle = !IsDeletedToggle;
                return RedirectToPage("./Index", new { isDeletedToggle = IsDeletedToggle });
            }
            else
            {
                return RedirectToPage("./Create");
            }
        }

        private async Task<IList<Product>> LoadProducts(bool isDeleted)
        {
            var products = await _productManager.GetProductsAsync(isDeleted);

            return products;
        }
    }
}
