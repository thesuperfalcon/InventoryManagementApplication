using InventoryManagementApplication.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagementApplication.Pages
{
    [Authorize]
    public class AboutUsModel : PageModel
    {
        private readonly UserManager _userManager;

        public AboutUsModel(UserManager userManager)
        {
            _userManager = userManager;
        }
        [BindProperty]
        public List<Models.Developer> Developers { get; set; }
        public async Task OnGetAsync()
        {
            Developers = await _userManager.GetAllDevelopersAsync();
        }
    }
}
