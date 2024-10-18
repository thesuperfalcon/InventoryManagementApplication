using System.Text.Json.Serialization;
#nullable enable

namespace InventoryManagementApplication.Models
{
    public class Statistic
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("userId")]
        public string? UserId { get; set; }

        [JsonPropertyName("userName")]
        public string? UserName { get; set; }

        [JsonPropertyName("employeeNumber")]
        public string? EmployeeNumber { get; set; }

        [JsonPropertyName("productId")]
        public int? ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string? ProductName { get; set; }

        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }

        [JsonPropertyName("initialStorageId")]
        public int? InitialStorageId { get; set; }

        [JsonPropertyName("initialStorageName")]
        public string? IntitialStorageName { get; set; }

        [JsonPropertyName("destinationStorageId")]
        public int? DestinationStorageId { get; set; }

        [JsonPropertyName("destinationStorageName")]
        public string? DestinationStorageName { get; set; }

        [JsonPropertyName("moved")]
        public DateTime? Moved { get; set; } = DateTime.Now;

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }
    }
}
