using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using System.Text.Json;

namespace InventoryManagementApplication.Pages.admin.storage
{
    public class CreateModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public CreateModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        private static Uri BaseAddress = new Uri("https://localhost:44353/");

        [BindProperty]
        public Storage Storage { get; set; } = default!;
        public string StatusMessage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                var json = JsonSerializer.Serialize(Storage);

                //Gör det möjligt att skicka innehåll till API
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/Storages/", httpContent);
            }

            //_context.Storages.Add(Storage);
            //await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            else
            {
                StatusMessage = "Max antal lagerplatser är 6";
                return Page();
            }
        }
    }
}
