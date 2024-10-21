using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InventoryManagementApplication.Models;
using System.Text.Json;
using InventoryManagementApplication.DAL;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementApplication.Pages.admin.storage
{
    [Authorize]
    public class DetailsModel : PageModel
    {
		private readonly StorageManager _manager;

        public DetailsModel(StorageManager manager)
        {
			_manager = manager;
        }
		public Storage Storage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
			Storage = await _manager.GetStorageByIdAsync(id, null);

			if (Storage == null)
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