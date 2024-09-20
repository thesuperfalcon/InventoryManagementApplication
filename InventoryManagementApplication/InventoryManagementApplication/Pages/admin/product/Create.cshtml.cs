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

namespace InventoryManagementApplication.Pages.admin.product
{
    public class CreateModel : PageModel
    {
		private readonly ProductManager _manager;

		public CreateModel(ProductManager manager)
		{
			_manager = manager;
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
						
			return RedirectToPage("./Index");
		}
	}
    
}
