using System.Text.Json.Serialization;
#nullable enable

namespace InventoryManagementApplication.Models
{
    public record Log
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("userId")]
        public string? UserId { get; set; } 

		[JsonPropertyName("userName")] 
		public string? UserName { get; set; }

        [JsonPropertyName("userFullName")]
        public string? UserFullName { get; set; } 

        [JsonPropertyName("employeeNumber")]
		public string? EmployeeNumber { get; set; }

        [JsonPropertyName("action")]
        public string? Action { get; set; }

        [JsonPropertyName("entityId")]
		public int? EntityId { get; set; }

		[JsonPropertyName("entityType")]
		public string? EntityType { get; set; }

        [JsonPropertyName("entityName")]
        public string? EntityName { get; set; }

        [JsonPropertyName("entityDetails")]
        public string? EntityDetails { get; set; }

        [JsonPropertyName("timeStamp")]
        public DateTime? TimeStamp { get; set; } = DateTime.Now;
    }
}