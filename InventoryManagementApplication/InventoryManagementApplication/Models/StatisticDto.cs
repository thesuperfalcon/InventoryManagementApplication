using System.Text.Json.Serialization;

namespace InventoryManagementApplication.Models
{
    public class StatisticDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("orderTime")]
        public DateTime? OrderTime { get; set; }

        [JsonPropertyName("productQuantity")]
        public int ProductQuantity { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; }

        [JsonPropertyName("price")]
        public decimal? Price { get; set; }

        [JsonPropertyName("currentStock")]
        public int? CurrentStock { get; set; }

        [JsonPropertyName("ínitialStorageName")]
        public string InitialStorageName { get; set; }

        [JsonPropertyName("initialMaxCapacity")]
        public int? InitialMaxCapacity { get; set; }

        [JsonPropertyName("destinationStorageName")]
        public string DestinationStorageName { get; set; }

        [JsonPropertyName("destinationMaxCapacity")]
        public int? DestinationMaxCapacity { get; set; }

        [JsonPropertyName("reporterName")]
        public string ReporterName { get; set; }

        [JsonPropertyName("reporterEmployeeNumber")]
        public string ReporterEmployeeNumber { get; set; }
    }
}
