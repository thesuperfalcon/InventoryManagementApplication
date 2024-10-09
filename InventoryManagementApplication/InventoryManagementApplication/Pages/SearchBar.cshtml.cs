using InventoryManagementApplication.Areas.Identity.Data;
using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            Products = new List<Product>();
            Storages = new List<Storage>();
            Users = new List<InventoryManagementUser>();

            

            if (!string.IsNullOrWhiteSpace(query))
            {
                Products = await _productManager.SearchProductsAsync(query, null);
                Storages = await _storageManager.SearchStoragesAsync(query);
                Users = await _userManager.SearchUsersAsync(query, null);
            }

            Query = query;
        }
    }

}
