using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Text.Json;
using InventoryManagementApplication.DAL;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.WebUtilities;


namespace InventoryManagementApplication.Pages.admin.product
{
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
		public string StatusMessage { get; set; }

		[BindProperty]
		public Product Product { get; set; } //= default!;
		public List<string> ArticleNumbers { get; set; } 

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
			Product.CurrentStock = Product.TotalStock;
			Product.Created = DateTime.Now;
			var getProduct = await _manager.GetProductsAsync(null);




			ArticleNumbers = getProduct.Select(p => p.ArticleNumber).ToList();

			if (ArticleNumbers != null)
			{


				if (ArticleNumbers.Contains(Product.ArticleNumber))
				{
					var checkProduct = getProduct.Where(x => x.ArticleNumber == Product.ArticleNumber).FirstOrDefault();
					if (checkProduct.IsDeleted == true)
					{
						StatusMessage = "Artikelnummer finns redan bland borttagna produkter! Välj annan nummer.";
						return RedirectToPage("./Create");
					}
					StatusMessage = "Artikelnummer finns redan! Välj annat nummer.";
					return RedirectToPage("./Create");
				}
				else
				{
					await _manager.CreateProductAsync(Product);
					await _logManager.LogActivityAsync(Product, EntityState.Added);

				}

			}



			return RedirectToPage("./Index");
		}

   //     public async Task<IActionResult> OnGetCheckArticleNumberExists(string articleNumber)
   //     {
			
   //         var products = await _manager.GetProductsAsync(false);
			//bool exist = products.Any(a => a.ArticleNumber == articleNumber);
   //         return new JsonResult(!exists);
   //     }
    }
    
}
