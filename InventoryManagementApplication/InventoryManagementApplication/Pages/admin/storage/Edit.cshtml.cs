using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using System.Text.Json;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System.Text;

namespace InventoryManagementApplication.Pages.admin.storage
{
    public class EditModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public EditModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		
		[TempData] //Nödvändig????
		public string StatusMessage { get; set; }
		[BindProperty]
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
				//var storage = await _context.Storages.FirstOrDefaultAsync(m => m.Id == id);
				if (storage == null)
				{
					return NotFound();
				}
				Storage = storage;
				return Page();
			}
            
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
			int id = Storage.Id;
            using (var client = new HttpClient())
            {
				client.BaseAddress = BaseAddress;
				if (!ModelState.IsValid)
				{
					return Page();
				}
				//Ignorera????
				_context.Attach(Storage).State = EntityState.Modified;

				try
				{
					var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(Storage), Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PutAsync($"api/Storages/{id}", content);

					if (!response.IsSuccessStatusCode)
					{
						string responseContent = await response.Content.ReadAsStringAsync();
						StatusMessage = $"Failed to update product. Status: {response.StatusCode}, Reason: {response.ReasonPhrase}, Details: {responseContent}";
						return RedirectToPage("./Edit", new { id = Storage.Id });
					}
					//await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!StorageExists(Storage.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}

				
			}
			return RedirectToPage("./Index");
		}

        private bool StorageExists(int id)
        {
            return _context.Storages.Any(e => e.Id == id);
        }
    }
}
