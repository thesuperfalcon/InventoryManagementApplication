using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace InventoryManagementApplication.Areas.Identity.Data
{
    public class InventoryManagementRole : IdentityRole 
    {
        [JsonPropertyName("roleName")]
        public string RoleName { get; set; }

		[JsonPropertyName("fullAccess")]
		public bool FullAccess { get; set; }
    }
}
