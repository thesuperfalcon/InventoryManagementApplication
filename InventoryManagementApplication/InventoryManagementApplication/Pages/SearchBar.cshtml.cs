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

        public async Task<IActionResult> OnGetAsync(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                Query = query;
                Products = await _productManager.SearchProductsAsync(query);
                Storages = await _storageManager.SearchStoragesAsync(query);
                Users = await _userManager.SearchUsersAsync(query);
            }

            return Page();
        }

        public async Task<IActionResult> OnGetSearchSuggestionsAsync(string query)
        {

            if (string.IsNullOrWhiteSpace(query))
            {
                return new JsonResult(new List<Object>());
            }

            var productSuggestions = await _productManager.SearchProductsAsync(query);
            var storageSuggestions = await _storageManager.SearchStoragesAsync(query);
            var userSuggestions = await _userManager.SearchUsersAsync(query);

            // Combine the results into a single list

            var suggestions = productSuggestions.Select(p => new
            {
                name = p.Name,
                secondValue = $"{p.Name} - {p.ArticleNumber} - {p.Description} - {p.Price} - {p.TotalStock}"
            })
                                .Concat(storageSuggestions.Select(s => new { name = s.Name, secondValue = $"{s.Name} - {s.CurrentStock}/{s.MaxCapacity}" }))
                                .Concat(userSuggestions.Select(u => new { name = $"{u.FirstName} {u.LastName}", secondValue = $"{u.FirstName} {u.LastName} - {u.EmployeeNumber} - ({u.UserName})" }))
                                .ToList();

            return new JsonResult(suggestions);
        }
    }

}
