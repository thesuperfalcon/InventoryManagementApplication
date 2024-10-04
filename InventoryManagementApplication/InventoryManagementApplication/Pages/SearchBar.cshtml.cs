using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagementApplication.Pages
{
    public class SearchBarModel : PageModel
    {
        private readonly ProductManager _productManager;
        private readonly StorageManager _storageManager;
        private readonly UserManager _userManager;

        public List<Product> Products { get; set; }
        public List<Storage> Storages { get; set; }
        public List<InventoryManagementUser> Users { get; set; }

        public string Query { get; set; } 

        public SearchBarModel(ProductManager productManager, StorageManager storageManager, UserManager userManager)
        {
            _productManager = productManager;
            _storageManager = storageManager;
            _userManager = userManager;
        }

        public async Task OnGetAsync(string query)
        {
            Products = await _productManager.GetProductsAsync(false);
            Storages = await _storageManager.GetStoragesAsync(false);
            Users = await _userManager.GetAllUsersAsync();

            Query = query;

            if (!string.IsNullOrWhiteSpace(Query))
            {
                Products = Products
                    .Where(p => p.Id != null && p.Name.Contains(Query, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Storages = Storages
                    .Where(s => s.Name != null && s.Name.Contains(Query, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Users = Users
                    .Where(u => u.FirstName != null && u.LastName.Contains(Query, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                Products = await _productManager.GetProductsAsync(false);
                Storages = await _storageManager.GetStoragesAsync(false);
                Users = await _userManager.GetAllUsersAsync();
            }
        }

    }

}
