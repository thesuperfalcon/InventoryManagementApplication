using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InventoryManagementApplication.Models
{
    public class ActivityLog
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("userId")]
        public string? UserId { get; set; }

        [JsonPropertyName("action")]
        public ActionType? Action { get; set; }

        [JsonPropertyName("itemType")]
        public ItemType? ItemType { get; set; }

        [JsonPropertyName("typeId")]
        public int? TypeId { get; set; }

        [JsonPropertyName("timeStamp")]
        public DateTime? TimeStamp { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }

        [ForeignKey(nameof(TypeId))]
        public virtual Product? Product { get; set; }

        [ForeignKey(nameof(TypeId))]
        public virtual Storage? Storage { get; set; }

        public virtual InventoryManagementUser? User { get; set; }
    }

    public enum ActionType
    {
        Created,
        Updated,
        Deleted,
        Moved
    }

    public enum ItemType
    {
        Product,
        Storage
    }
}
