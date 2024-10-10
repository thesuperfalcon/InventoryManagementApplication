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
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementApplication.Pages.admin.product
{
    [Authorize]
    public class DetailsModel : PageModel
    {
		private readonly ProductManager _manager;

        public DetailsModel(ProductManager manager)
        {
			_manager = manager;
        }
		public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
			Product = await _manager.GetProductByIdAsync(id, null);

			if (Product == null)
			{
				return NotFound();
			}
			else
			{
				return Page();
			}		
		}
	}  
}
