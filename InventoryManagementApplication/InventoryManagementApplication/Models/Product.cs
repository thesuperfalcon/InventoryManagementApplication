using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
#nullable enable

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
        public DateTime? Created { get; set; } = DateTime.Now;

        [JsonPropertyName("updated")]
        public DateTime? Updated { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool? IsDeleted { get; set; } = false;

        [JsonPropertyName("inventoryTrackers")]
        public virtual ICollection<InventoryTracker> InventoryTrackers { get; set; } = new List<InventoryTracker>();
    }
}
