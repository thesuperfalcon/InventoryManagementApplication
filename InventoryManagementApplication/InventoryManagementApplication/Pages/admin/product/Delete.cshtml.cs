using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InventoryManagementApplication.Pages.admin.product
{
	public class DeleteModel : PageModel
	{
		private readonly InventoryManagementApplicationContext _context;

<<<<<<< Updated upstream
		public DeleteModel(InventoryManagementApplicationContext context)
		{
			_context = context;
		}
=======
        public DeleteModel(InventoryManagementApplicationContext context)
        {
            _context = context;
        }
        private static Uri BaseAddress = new Uri("https://localhost:44353/");
>>>>>>> Stashed changes

		private static Uri BaseAddress = new Uri("https://localhost:44353/");

		[BindProperty]
		public Product Product { get; set; } = default!;
		public List<InventoryTracker> InventoryTrackers { get; set; } = new List<InventoryTracker>();
		public List<Storage> Storages { get; set; } = new List<Storage>();

<<<<<<< Updated upstream
		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
=======

            Product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
>>>>>>> Stashed changes


			Product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

<<<<<<< Updated upstream
			if (Product == null)
			{
				return NotFound();
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{

			if (id == null)
			{
				return NotFound();
			}
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				

				HttpResponseMessage responseInventoryTracker = await client.GetAsync("api/InventoryTrackers");
				if (responseInventoryTracker.IsSuccessStatusCode)
				{
					string responseString = await responseInventoryTracker.Content.ReadAsStringAsync();
					List<Models.InventoryTracker> inventoryTrackers = JsonSerializer.Deserialize<List<Models.InventoryTracker>>(responseString);
					InventoryTrackers = inventoryTrackers.Where(x => x.ProductId == id).ToList();
=======
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (var client = new HttpClient())
            {
            client.BaseAddress = BaseAddress;
                var response = await client.DeleteAsync($"api/Products/{id}");
                //client.BaseAddress = BaseAddress;
                //var json = JsonSerializer.Serialize(Product);

                //Gör det möjligt att skicka innehåll till API
                //StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                //HttpResponseMessage response = await client.PostAsync("api/Products/", httpContent);
            }
            return RedirectToPage("./Index");
            //Product = await _context.Products.FindAsync(id);
            //if (Product == null)
            //{
            //    return NotFound();
            //}


            //InventoryTrackers = await _context.InventoryTracker
            //    .Where(x => x.ProductId == Product.Id)
            //    .ToListAsync();

            //Storages = await _context.Storages
            //    .Where(storage => storage.InventoryTrackers.Any(tracker => InventoryTrackers.Contains(tracker)))
            //    .ToListAsync();

            //foreach (var tracker in InventoryTrackers)
            //{
            //    var storage = Storages.FirstOrDefault(s => s.Id == tracker.StorageId);
            //    if (storage != null)
            //    {
            //        storage.CurrentStock -= tracker.Quantity; 
            //        _context.Storages.Update(storage); 
            //    }
            //}
>>>>>>> Stashed changes

				}

				HttpResponseMessage responseStorage = await client.GetAsync("api/Storages");
				if (responseStorage.IsSuccessStatusCode)
				{
					string responseString = await responseStorage.Content.ReadAsStringAsync();
					List<Models.Storage> storages = JsonSerializer.Deserialize<List<Models.Storage>>(responseString);
					int trackerId = (int)id;
					//Be om förklaring någon gång, Storages blir null?
					//Deklarera Product, hämta ur Lista med samma id
					Storages = storages.Where(storage => storage.InventoryTrackers.Any(tracker => InventoryTrackers.Contains(tracker))).ToList();

				}

				foreach (var tracker in InventoryTrackers)
				{
					Models.Storage storage = Storages.FirstOrDefault(s => s.Id == tracker.StorageId);
					if (storage != null)
					{
						storage.CurrentStock -= tracker.Quantity;

						var json = JsonSerializer.Serialize(storage);

						StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
						HttpResponseMessage responseUpdateStorage = await client.PutAsync("api/Storages/" + storage.Id, httpContent);
					}
				
					
				}
				var response = await client.DeleteAsync($"api/Products/{id}");


			}
				return RedirectToPage("./Index");
		}
	}
}


//InventoryTrackers = await _context.InventoryTracker
//    .Where(x => x.ProductId == Product.Id)
//    .ToListAsync();

//Storages = await _context.Storages
//    .Where(storage => storage.InventoryTrackers.Any(tracker => InventoryTrackers.Contains(tracker)))
//    .ToListAsync();


//}