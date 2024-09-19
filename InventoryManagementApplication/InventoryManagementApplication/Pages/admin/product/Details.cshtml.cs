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
using InventoryManagementApplication.DAL;

namespace InventoryManagementApplication.Pages.admin.product
{
    public class DetailsModel : PageModel
    {
        private readonly InventoryManagementApplication.Data.InventoryManagementApplicationContext _context;
		private readonly ProductManager _manager;

        public DetailsModel(InventoryManagementApplication.Data.InventoryManagementApplicationContext context, ProductManager manager)
        {
            _context = context;
			_manager = manager;

        }
		public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
			Product = await _manager.GetOneProductAsync(id);


			//         using (var client = new HttpClient())
			//         {
			//	client.BaseAddress = BaseAddress;
			//	if (id == null)
			//	{
			//		return NotFound();
			//	}
			//	HttpResponseMessage response = await client.GetAsync($"api/Products/");
			//	if(response.IsSuccessStatusCode)
			//	{
			//		string responseString = await response.Content.ReadAsStringAsync();
			//		products = JsonSerializer.Deserialize<List<Models.Product>>(responseString);
			//	}

			if (Product == null)
			{
				return NotFound();
			}
			else
			{
				return Page();
				//Product = products.Where(x => x.Id == id).SingleOrDefault();
			}

			
		}

	}
    
}
