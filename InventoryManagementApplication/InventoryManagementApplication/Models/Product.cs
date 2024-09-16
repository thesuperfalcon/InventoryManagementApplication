using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventoryManagementApplication.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("articleNumber")]
        public string? ArticleNumber { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("price")]
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }

        [JsonPropertyName("totalStock")]
        public int? TotalStock { get; set; }

        [JsonPropertyName("currentStock")]
        public int? CurrentStock { get; set; }

		[JsonPropertyName("created")]
		public DateTime? Created { get; set; }

		[JsonPropertyName("updated")]
		public DateTime? Updated { get; set; }

        public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

        public virtual ICollection<InventoryTracker> InventoryTrackers { get; set; } = new List<InventoryTracker>();

        public virtual ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();
    }
}
