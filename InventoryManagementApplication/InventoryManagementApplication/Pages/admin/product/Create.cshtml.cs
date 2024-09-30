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

namespace InventoryManagementApplication.Pages.admin.product
{
    public class CreateModel : PageModel
    {
		private readonly ProductManager _manager;
		private readonly ActivityLogManager _logManager;

		public CreateModel(ProductManager manager, ActivityLogManager logManager)
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
			await _logManager.LogActivityAsync(Product, EntityState.Added);
						
			return RedirectToPage("./Index");
		}
	}
    
}
