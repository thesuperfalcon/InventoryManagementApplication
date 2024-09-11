using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace InventoryManagementApplication.Pages
{
	
    public class TemporaryAdminModel : PageModel
    {
		private readonly HttpClient _httpClient;
	

		public TemporaryAdminModel(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public IList<Models.Product> Products { get; set; } 
		public IList<Models.Storage> Storages { get; set; }
		public Models.Product CreateProduct { get; set; }

		public async Task OnGetAsync()
		{
			//Kan behöva ändra localhost strängen innan vi lägger upp det här på molnet 
			Products = await _httpClient.GetFromJsonAsync<IList<Models.Product>>("https://localhost:44353/api/Products");
			Storages = await _httpClient.GetFromJsonAsync<IList<Models.Storage>>("https://localhost:44353/api/Storages");
		}
	}
}
