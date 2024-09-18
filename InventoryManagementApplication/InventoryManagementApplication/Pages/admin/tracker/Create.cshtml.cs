using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Text.Json;

namespace InventoryManagementApplication.Pages.admin.tracker
{
    public class CreateModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public CreateModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }
        private static Uri BaseAddress = new Uri("https://localhost:44353/");
        [TempData]
        public string StatusMessage { get; set; }
        public SelectList StorageSelectList { get; set; }
        public SelectList ProductSelectList { get; set; }
        [BindProperty]
        public InventoryTracker InventoryTracker { get; set; } = default!;
        public Models.Storage Storage { get; set; }
        public Product? Product { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                var products = new List<Models.Product>();
                var storages = new List<Models.Storage>();
                HttpResponseMessage responseProducts = await client.GetAsync($"api/Products/");
                if (responseProducts.IsSuccessStatusCode)
                {
                    string responseString = await responseProducts.Content.ReadAsStringAsync();
                    products = JsonSerializer.Deserialize<List<Models.Product>>(responseString);
                }

                HttpResponseMessage responseStorage = await client.GetAsync($"api/Storages/");
                if (responseStorage.IsSuccessStatusCode)
                {
                    string responseString = await responseStorage.Content.ReadAsStringAsync();
                    storages = JsonSerializer.Deserialize<List<Models.Storage>>(responseString);
                }
                
                //return RedirectToPage("./Index");
                //Allt returnerar null, får inte upp några alternativ på Create Tracker
                //var storages = await _context.Storages.ToListAsync();
                //var products = await _context.Products.ToListAsync();

                var productItems = products.Select(x => new
                {
                    Value = x.Id,
                    Text = $"{x.Name} (Antal utan lager: {x.CurrentStock})"
                });

                var storageItems = storages.Select(x => new
                {
                    Value = x.Id,
                    Text = $"{x.Name} (Lediga platser: {x.MaxCapacity - x.CurrentStock})"
                });

                StorageSelectList = new SelectList(storageItems, "Value", "Text");
                ProductSelectList = new SelectList(productItems, "Value", "Text");
            }
            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                ////Product.CurrentStock = Product.TotalStock;
                //if (Product.TotalStock != null)
                //{
                //    int? totalStock = Product.TotalStock;
                //    int? currentStock = Product.CurrentStock;
                //    currentStock = totalStock;
                //    Product.CurrentStock = currentStock;
                //}

                if (!ModelState.IsValid)
                {
                    return Page();
                }
                var existingTracker = new InventoryTracker();
                var products = new List<Models.Product>();

                int quantity = 0;
                HttpResponseMessage responseTrackers = await client.GetAsync($"api/InventoryTrackers/");
                if (responseTrackers.IsSuccessStatusCode)
                {
                    string responseString = await responseTrackers.Content.ReadAsStringAsync();
                    existingTracker = JsonSerializer.Deserialize<List<Models.InventoryTracker>>(responseString).Where(x => x.ProductId == InventoryTracker.ProductId && x.StorageId ==
                    InventoryTracker.StorageId).FirstOrDefault(); 
                    
                }

                //var existingTracker = await _context.InventoryTracker.Where(x => x.ProductId == InventoryTracker.ProductId && x.StorageId == InventoryTracker.StorageId).FirstOrDefaultAsync();
                if (existingTracker == null)
                {
                    var json = JsonSerializer.Serialize(InventoryTracker);

                    //Gör det möjligt att skicka innehåll till API
                    StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("api/InventoryTrackers/", httpContent);
                    //_context.InventoryTracker.Add(InventoryTracker);
                    quantity = (int)InventoryTracker.Quantity;
                    //await _context.SaveChangesAsync();
                }
                else
                {
                    quantity = (int)InventoryTracker.Quantity;
                }

                HttpResponseMessage responseProducts = await client.GetAsync($"api/InventoryTrackers/");
                if (responseProducts.IsSuccessStatusCode)
                {
                    string responseString = await responseProducts.Content.ReadAsStringAsync();
                    Product = JsonSerializer.Deserialize<List<InventoryTracker>>(responseString).Select(x => x.Product)
                    .FirstOrDefault(x => x.Id == InventoryTracker.ProductId);
                }

                HttpResponseMessage responseStorage = await client.GetAsync($"api/InventoryTrackers/");
                if (responseStorage.IsSuccessStatusCode)
                {
                    string responseString = await responseStorage.Content.ReadAsStringAsync();
                    Storage = JsonSerializer.Deserialize<List<InventoryTracker>>(responseString).Select(x => x.Storage)
                   .FirstOrDefault(x => x.Id == InventoryTracker.StorageId); ;
                }

                //Storage = await _context.InventoryTracker
                //    .Select(x => x.Storage)
                //    .Where(x => x.Id == InventoryTracker.StorageId)
                //    .FirstOrDefaultAsync();

                //Product = await _context.InventoryTracker
                //    .Select(x => x.Product)
                //    .Where(x => x.Id == InventoryTracker.ProductId)
                //    .FirstOrDefaultAsync();

                var currentSpace = Storage.MaxCapacity - Storage.CurrentStock;
                
                if (currentSpace < quantity)
                {

                    StatusMessage = $"Finns ej plats i {Storage.Name}. Välj annan lagerplats!";
                    if (existingTracker == null)
                    {
                        var json = JsonSerializer.Serialize(InventoryTracker);

                        //Gör det möjligt att skicka innehåll till API
                        StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync("api/InventoryTrackers/", httpContent);
                        //_context.Remove(InventoryTracker);
                        //await _context.SaveChangesAsync();
                    }
                    return RedirectToPage("./Create");

                }


                if (quantity > Product.CurrentStock)
                {
                    int id = InventoryTracker.Id;

                    StatusMessage = $"Antalet produkter du vill lägga till finns ej tillgänglig. Antal produkter utan lager: {Product.CurrentStock}";
                    if (InventoryTracker != null && InventoryTracker.Id != 0)
                    {
                        var json = JsonSerializer.Serialize(InventoryTracker);

                        //Gör det möjligt att skicka innehåll till API
                        StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.DeleteAsync($"api/InventoryTrackers/{id}");
                        //_context.InventoryTracker.Remove(InventoryTracker);
                        //await _context.SaveChangesAsync();
                    }
                    return RedirectToPage("./Create");
                }
                else if (Storage.CurrentStock < quantity)
                {
                    StatusMessage = $"Finns ej plats i {Storage.Name}. Välj annan lagerplats!";
                }

                if (existingTracker != null)
                {
                    existingTracker.Quantity += InventoryTracker.Quantity;
                }
                Storage.CurrentStock += quantity;
                Product.CurrentStock -= quantity;
                Storage.Updated = DateTime.Now;
                //await _context.SaveChangesAsync();

            }
            return RedirectToPage("./Index");
        }
    }
}
