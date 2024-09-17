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
    public class IndexModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public IndexModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public IList<Storage> Storages { get;set; } = default!;

        public async Task OnGetAsync()
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

				HttpResponseMessage responseStorages = await client.GetAsync("api/Storages/");
				if (responseStorages.IsSuccessStatusCode)
				{
					string responseString = await responseStorages.Content.ReadAsStringAsync();
					List<Models.Storage> storages = JsonSerializer.Deserialize<List<Models.Storage>>(responseString);
					Storages = storages.ToList();

				}
				//Storage = await _context.Storages.ToListAsync();
			}
            
        }
    }
}
