using InventoryManagementApplication.DAL;
using InventoryManagementApplication.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Helpers
{
	public class SelectListHelpers
	{
		private readonly InventoryManagementApplicationContext _context;
		private readonly ProductManager _productManager;
		private readonly StorageManager _storageManager;
		public SelectListHelpers(InventoryManagementApplicationContext context, ProductManager productManager,
			StorageManager storageManager)
		{
			_context = context;
			_productManager = productManager;
			_storageManager = storageManager;
		}

		public async Task<SelectList> GenerateProductSelectListAsync()
		{
			var products = await _productManager.GetAllProductsAsync();
			//var products = await _context.Products.ToListAsync();

			var productItems = products.Select(x => new
			{
				Value = x.Id,
				Text = $"{x.Name} (Antal utan lager: {x.CurrentStock})"
			});

			return new SelectList(productItems, "Value", "Text");
		}

		public async Task<SelectList> GenerateStorageSelectListAsync(int? storageId)
		{
			var storages = await _storageManager.GetAllStoragesAsync();
			//var storages = await _context.Storages.ToListAsync();

			var storageItems = storages
				.Select(x => new
				{
					Value = x.Id,
					Text = $"{x.Name} (Lediga platser: {x.MaxCapacity - x.CurrentStock})"
				})
				.ToList();

			if (storageId.HasValue && storageId.Value != 0)
			{
				storageItems = storageItems
					.Where(x => x.Value != storageId.Value)
					.ToList();
			}

			return new SelectList(storageItems, "Value", "Text");
		}

	}
}
