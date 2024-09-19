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

namespace InventoryManagementApplication.Pages.admin.product
{
    public class DetailsModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;

        public DetailsModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context)
        {
            _context = context;
        }
		private static Uri BaseAddress = new Uri("https://localhost:44353/");
		public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
			var  products = new List<Product>();
            using (var client = new HttpClient())
            {
				client.BaseAddress = BaseAddress;
				if (id == null)
				{
					return NotFound();
				}
				HttpResponseMessage response = await client.GetAsync($"api/Products/");
				if(response.IsSuccessStatusCode)
				{
					string responseString = await response.Content.ReadAsStringAsync();
					products = JsonSerializer.Deserialize<List<Models.Product>>(responseString);
				}

				if (products == null)
				{
					return NotFound();
				}
				else
				{
					Product = products.Where(x => x.Id == id).SingleOrDefault();
				}

				return Page();
			}
            
        }
    }
}
