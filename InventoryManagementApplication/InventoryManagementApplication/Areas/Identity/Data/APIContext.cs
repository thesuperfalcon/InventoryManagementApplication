using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApplication.Areas.Identity.Data
{
	public class APIContext : DbContext
	{
		public APIContext(DbContextOptions<APIContext> options)
		: base(options)
		{

		}


	}
}
