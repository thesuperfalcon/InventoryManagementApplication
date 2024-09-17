using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InventoryManagementApplication.Pages.admin.storage
{
    public class DeleteModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public DeleteModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        private static Uri BaseAddress = new Uri("https://localhost:44353/");

        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public Storage Storage { get; set; } = default!;
        public List<InventoryTracker> Trackers { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storage = new Storage();
            //var storage = await _context.Storages.FirstOrDefaultAsync(m => m.Id == id);

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;


                HttpResponseMessage responseStorage = await client.GetAsync("api/Storages");
                if (responseStorage.IsSuccessStatusCode)
                {
                    string responseString = await responseStorage.Content.ReadAsStringAsync();
                    storage = JsonSerializer.Deserialize<List<Models.Storage>>(responseString).Where(s => s.Id == id).FirstOrDefault();


                }

                if (storage == null)
                {
                    return NotFound();
                }
                else
                {
                    Storage = storage;
                }
                return Page();
            }


        }
            public async Task<IActionResult> OnPostAsync(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }
            var storage = new Storage();
            // var storage = await _context.Storages.FindAsync(id);
            //Trackers = await _context.InventoryTracker.Where(x => x.StorageId == storage.Id).ToListAsync();
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                HttpResponseMessage responseStorage = await client.GetAsync($"api/Storage/{id}");
                if (responseStorage.IsSuccessStatusCode)
                {
                    string responseString = await responseStorage.Content.ReadAsStringAsync();
                    storage = JsonSerializer.Deserialize<Storage>(responseString);
                    // InventoryTrackers = inventoryTrackers.Where(x => x.ProductId == id).ToList();

                }


                HttpResponseMessage responseInventoryTracker = await client.GetAsync($"api/InventoryTrackers");
                if (responseInventoryTracker.IsSuccessStatusCode)
                {
                    string responseString = await responseInventoryTracker.Content.ReadAsStringAsync();
                    Trackers = JsonSerializer.Deserialize<List<InventoryTracker>>(responseString).Where(x => x.StorageId == storage.Id).ToList();
                    // InventoryTrackers = inventoryTrackers.Where(x => x.ProductId == id).ToList();

                }




                var sum = Trackers.Sum(x => x.Quantity);
                if (sum > 0)
                {
                    StatusMessage = "Går ej att ta bort. Flytta produkterna innann du tar bort lagret!";

                    return RedirectToPage("./Delete", new { id = storage.Id });
                }


                if (storage != null)
                {
                    var response = await client.DeleteAsync($"api/Storages/{id}");

                    //_context.Storages.Remove(Storage);
                    //await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
            }
        }
    }
