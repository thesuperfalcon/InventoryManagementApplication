using InventoryManagementApplication.Areas.Identity.Data;
using System.Text.Json.Serialization;

namespace InventoryManagementApplication.DTO
{
    public class RolesDTO
    {
        [JsonPropertyName("user")]
        public InventoryManagementUser? User { get; set; }

        [JsonPropertyName("currentRoles")]
        public List<string?>? CurrentRoles { get; set; }

        [JsonPropertyName("addRole")]
        public string? AddRole { get; set; }
    }
}
