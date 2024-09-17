using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using System.Text.Json;

namespace InventoryManagementApplication.Pages.admin.storage
{
    public class DetailsModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public DetailsModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }
        private static Uri BaseAddress = new Uri("https://localhost:44353/");
        public Storage Storage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var storage = new Storage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                if (id == null)
                {
                    return NotFound();
                }
                HttpResponseMessage response = await client.GetAsync($"api/Storages/");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    storage = JsonSerializer.Deserialize<List<Models.Storage>>(responseString).Where(s => s.Id == id).SingleOrDefault();
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

            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var storage = await _context.Storages.FirstOrDefaultAsync(m => m.Id == id);
            //if (storage == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    Storage = storage;
            //}
            //return Page();
        }
    }
}
