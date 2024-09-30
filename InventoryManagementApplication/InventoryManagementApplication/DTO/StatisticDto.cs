using System.Text.Json.Serialization;

namespace InventoryManagementApplication.DTO
{
    public class StatisticDto
    {
        [JsonPropertyName("userId")]
        public string? UserId { get; set; }

        [JsonPropertyName("initialStorageId")]
        public int? InitialStorageId { get; set; }

        [JsonPropertyName("destinationStorageId")]
        public int? DestinationStorageId { get; set; }

        [JsonPropertyName("productId")]
        public int? ProductId { get; set; }

        [JsonPropertyName("productQuantity")]
        public int ProductQuantity { get; set; }

        [JsonPropertyName("orderTime")]
        public DateTime? OrderTime { get; set; } = DateTime.Today;
    }
}
