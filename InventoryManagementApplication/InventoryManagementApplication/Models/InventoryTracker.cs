using InventoryManagementApplication.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementApplication.Models
{
    public class InventoryTracker
    {
        public int Id { get; set; }

        public int? StorageId { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

        public virtual Product? Product { get; set; }

        public virtual Storage? Storage { get; set; }
    }
}
