using InventoryManagementApplication.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InventoryManagementApplication.Models
{
    public class Statistic
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("userId")]
        public string? UserId { get; set; }

        [JsonPropertyName("User")]
        public virtual InventoryManagementUser? User { get; set; }

        [JsonPropertyName("intitialStorageId")]
        public int? InitialStorageId { get; set; }

        [JsonPropertyName("initialStorage")]
        public virtual Storage? InitialStorage { get; set; }

        [JsonPropertyName("destinationStorageId")]
        public int? DestinationStorageId { get; set; }

        [JsonPropertyName("destinationStorage")]
        public virtual Storage? DestinationStorage { get; set; }

        [JsonPropertyName("productId")]
        public int? ProductId { get; set; }

        [JsonPropertyName("productQuantity")]
        public int ProductQuantity { get; set; }

        [JsonPropertyName("product")]
        public virtual Product? Product { get; set; }

        [JsonPropertyName("orderTime")]
        public DateTime? OrderTime { get; set; }

        [JsonPropertyName("finishedTime")]
        public DateTime? FinishedTime { get; set; }

        [JsonPropertyName("completed")]
        public bool? Completed { get; set; }
        [JsonPropertyName("notes")]

        public string? Notes { get; set; }
    }
}
