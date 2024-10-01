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
		private readonly StorageManager _storageManager;
		private readonly LogManager _activityLogManager;
		public EditModel(ProductManager productManager, TrackerManager trackerManager, StorageManager storageManager, LogManager activityLogManager)
		{
			_productManager = productManager;
			_trackerManager = trackerManager;
			_storageManager = storageManager;
			_activityLogManager = activityLogManager;
		}

		[TempData]
		public string StatusMessage { get; set; }
		[BindProperty]
		public Product Product { get; set; } = default!;
		public List<InventoryTracker> InventoryTrackers { get; set; }

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

            var productNoChanges = await _productManager.GetProductByIdAsync(Product.Id, null);

            var defaultStorage = await _storageManager.GetDefaultStorageAsync();
			var tracker = await _trackerManager.GetTrackerByProductAndStorageAsync(Product.Id, defaultStorage.Id);

            //if (tracker == null)
            //{
            //    return RedirectToPage("./Edit", new { id = Product.Id });
            //}

			InventoryTrackers = await _trackerManager.GetTrackerByProductOrStorageAsync(Product.Id, 0);

			if (InventoryTrackers != null && InventoryTrackers.Count > 0)
			{
                var productQuantity = InventoryTrackers.Sum(x => x.Quantity);

                if (Product.TotalStock < productQuantity)
				{
					StatusMessage = $"Förbjudet att sänka antalet produkter";
					return RedirectToPage("./Edit", new { id = Product.Id });
				}
				else
				{
					var input = Product.TotalStock - productQuantity;
					Product.CurrentStock += input;
					defaultStorage.CurrentStock += input;
					tracker.Quantity += input;
				}
			}
			else
			{
				Product.CurrentStock = Product.TotalStock;
			}

			try
			{

				await _productManager.EditProductAsync(Product);
				await _storageManager.EditStorageAsync(defaultStorage);
				if (tracker != null)
				{
					await _trackerManager.EditTrackerAsync(tracker);
				}
				await _activityLogManager.LogActivityAsync(Product, EntityState.Modified, productNoChanges);
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
			var product = await _productManager.GetProductByIdAsync(id, false);

			return product != null ? true : false;
		}
	}
}
