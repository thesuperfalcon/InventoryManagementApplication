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

namespace InventoryManagementApplication.Pages.admin.product
{
    public class CreateModel : PageModel
    {
		private readonly ProductManager _manager;
		private readonly LogManager _logManager;
	
		public CreateModel(ProductManager manager, LogManager logManager)
		{
			_manager = manager;
			_logManager = logManager;
		}

		public IActionResult OnGet()
		{
			return Page();
		}

		[BindProperty]
		public Product Product { get; set; } = default!;

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
			Product.CurrentStock = Product.TotalStock;
			Product.Created = DateTime.Now;

			await _manager.CreateProductAsync(Product);

			var createdProduct = await _manager.GetProductByArticleNumberAsync(Product.ArticleNumber);

			if (createdProduct != null)
			{
                await _logManager.LogActivityAsync(createdProduct, EntityState.Added);
            }

			return RedirectToPage("./Index");
		}
	}
    
}
