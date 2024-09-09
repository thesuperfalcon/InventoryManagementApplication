using InventoryManagementApplication.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementApplication.Models
{
    public class Statistic
    {
        public int Id { get; set; }

        public string? ReporterId { get; set; }
        public virtual InventoryManagementUser? Reporter { get; set; }

        public string? ExecuterId { get; set; }
        public virtual InventoryManagementUser? Executer { get; set; }

        public int? InitialStorageId { get; set; }
        public virtual Storage? InitialStorage { get; set; }

        public int? DestinationStorageId { get; set; }
        public virtual Storage? DestinationStorage { get; set; }

        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public int ProductQuantity { get; set; }

        public DateTime? OrderTime { get; set; }
        public DateTime? FinishedTime { get; set; }
        public bool? Completed { get; set; }
        public string? Notes { get; set; }

    }
}
