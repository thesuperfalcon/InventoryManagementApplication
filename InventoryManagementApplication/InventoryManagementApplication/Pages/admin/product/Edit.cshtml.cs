using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.Pages.admin.product
{
	public class EditModel : PageModel
	{
		private readonly ProductManager _productManager;
		private readonly TrackerManager _trackerManager;
		public EditModel(ProductManager productManager, TrackerManager trackerManager)
		{
			_productManager = productManager;
			_trackerManager = trackerManager;
		}

		[TempData]
		public string StatusMessage { get; set; }
		[BindProperty]
		public Product Product { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _productManager.GetProductByIdAsync(id, null);

			if (product == null)
			{
				return NotFound();
			}

			Product = product;
			return Page();

		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
			List<InventoryTracker> inventoryTrackers = await _trackerManager.GetAllTrackersAsync();
			inventoryTrackers = inventoryTrackers.Where(p => p.ProductId == Product.Id).ToList();

			var productQuantity = inventoryTrackers.Sum(x => x.Quantity);

			if (inventoryTrackers != null && inventoryTrackers.Count > 0)
			{

				if (Product.TotalStock < productQuantity)
				{
					StatusMessage = $"Går ej att ändra. Currentstock: {Product.CurrentStock} Total: {Product.TotalStock}";
					return RedirectToPage("./Edit", new { id = Product.Id });
				}
				else
				{
					var input = Product.TotalStock - productQuantity;
					Product.CurrentStock = input;
				}
			}
			else
			{
				Product.CurrentStock = Product.TotalStock;
			}

			// Enligt AI så byter man ut den här mot PutAsync när det gäller API?
			//_context.Attach(Product).State = EntityState.Modified;

			try
			{
				await _productManager.EditProductAsync(Product);
				return RedirectToPage("./Edit", new { id = Product.Id });

			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await ProductExists(Product.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
		}
	
		private async Task<bool> ProductExists(int id)
		{
			var products = await _productManager.GetProductsAsync(false);
			
			return products.Any(e => e.Id == id);
		}
	}
	}
