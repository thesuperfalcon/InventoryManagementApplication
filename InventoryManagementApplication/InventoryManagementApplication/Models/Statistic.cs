using System.Text.Json.Serialization;

namespace InventoryManagementApplication.Models
{
    public class Statistic
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("user_name")]
        public string? UserName { get; set; }

        [JsonPropertyName("employee_number")]
        public string? EmployeeNumber { get; set; }

        [JsonPropertyName("product_id")]
        public int? ProductId { get; set; }

        [JsonPropertyName("product_name")]
        public string? ProductName { get; set; }

        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }

        [JsonPropertyName("initial_storage_id")]
        public int? InitialStorageId { get; set; }

        [JsonPropertyName("initial_storage_name")]
        public string? IntitialStorageName { get; set; }

        [JsonPropertyName("destination_storage_id")]
        public int? DestinationStorageId { get; set; }

        [JsonPropertyName("destination_storage_name")]
        public string? DestinationStorageName { get; set; }

        [JsonPropertyName("moved")]
        public DateTime? Moved { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }
    }
}
