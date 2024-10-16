using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;


namespace InventoryManagementApplication.Pages.admin.product
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ProductManager _manager;
        private readonly LogManager _logManager;
        private readonly bool exists;

        public CreateModel(ProductManager manager, LogManager logManager)
        {
            _manager = manager;
            _logManager = logManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        [TempData]
        public string StatusMessage3 { get; set; }

        [BindProperty]
        public Product Product { get; set; } //= default!;
        public List<string> ArticleNumbers { get; set; }

        [BindProperty]
        [Required]
        public string ArticleNumber { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingProductName = await _manager.CheckProductName(Product.Name);
            if(existingProductName == true)
            {
                StatusMessage3 = "Produkt finns med samma namn. Skriv in ett nytt namn";
                return Page();
            }

            Product.CurrentStock = Product.TotalStock;
            Product.Created = DateTime.Now;
            Product.ArticleNumber = ArticleNumber;
            var getProduct = await _manager.GetProductsAsync(null);

            ArticleNumbers = getProduct.Select(p => p.ArticleNumber).ToList();
           
            await _manager.CreateProductAsync(Product);
            await _logManager.LogActivityAsync(Product, EntityState.Added);


            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnPostGenerateArticleNumberAsync()
        {
            Random random = new Random();

            string articleNumber = random.Next(000000000, 999999999).ToString();

            var getProducts = await _manager.GetProductsAsync(null);

            ArticleNumbers = getProducts.Select(p => p.ArticleNumber).ToList();
            if (ArticleNumbers != null)
            {
                if (ArticleNumbers.Contains(articleNumber))
                {
                    OnPostGenerateArticleNumberAsync();
                }
            }
            ArticleNumber = articleNumber.ToString();

            return new JsonResult(new { articleNumber = articleNumber.ToString() });
        }

    }

}
