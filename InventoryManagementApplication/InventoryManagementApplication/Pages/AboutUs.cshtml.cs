using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagementApplication.Pages
{
    [Authorize]
    public class AboutUsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
