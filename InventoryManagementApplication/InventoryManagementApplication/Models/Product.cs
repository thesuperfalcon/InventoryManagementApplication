namespace InventoryManagementApplication.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? ArticleNumber { get; set; }

        public string? Description { get; set; }

        public double? Price { get; set; }

        public int? Stock { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }

        public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

        public virtual ICollection<InventoryTracker> InventoryTrackers { get; set; } = new List<InventoryTracker>();

        public virtual ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();
    }
}
