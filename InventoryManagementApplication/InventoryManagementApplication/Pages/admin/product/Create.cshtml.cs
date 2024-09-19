using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InventoryManagementApplication.Data;
using InventoryManagementApplication.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Text.Json;

namespace InventoryManagementApplication.Pages.admin.product
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
		public Product Product { get; set; } = default!;

		// For more information, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
			Product.CurrentStock = Product.TotalStock;
			using (var client = new HttpClient())
			{
				client.BaseAddress = BaseAddress;
				var json = JsonSerializer.Serialize(Product);

				//Gör det möjligt att skicka innehåll till API
				StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.PostAsync("api/Products/", httpContent);
			}
			return RedirectToPage("./Index");
		}
	}

        //private static Uri BaseAddress = new Uri("https://localhost:44353/");

        //[BindProperty]
        //public Product Product { get; set; } = default!;

        //// For more information, see https://aka.ms/RazorPagesCRUD.
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }
        //    Product.CurrentStock = Product.TotalStock;
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = BaseAddress;
        //        var json = JsonSerializer.Serialize(Product);

        //        //Gör det möjligt att skicka innehåll till API
        //        StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await client.PostAsync("api/Products/", httpContent);
        //    }
        //    return RedirectToPage("./Index");
        //}
    
}
