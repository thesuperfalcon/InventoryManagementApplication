using InventoryManagementApplication.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static InventoryManagementApplication.Pages.UsersRolesModel;

namespace InventoryManagementApplication.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public async void OnGet()
        {

        }
    }
}
