using Microsoft.AspNetCore.Identity;

namespace InventoryManagementApplication.Areas.Identity.Data
{
    public class InventoryManagementRole : IdentityRole 
    {
        public string RoleName { get; set; }
        public bool FullAccess { get; set; }
    }
}
