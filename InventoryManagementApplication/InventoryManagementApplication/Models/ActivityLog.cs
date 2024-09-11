using InventoryManagementApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementApplication.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public ActionType? Action { get; set; }

        public ItemType? ItemType { get; set; }

        public int? TypeId { get; set; }
        public DateTime? TimeStamp { get; set; }

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
