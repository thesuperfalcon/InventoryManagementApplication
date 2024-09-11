namespace InventoryManagementApplication.Models
{
    public class Storage
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? MaxCapacity { get; set; }

        public int? CurrentStock { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }

        public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

        public virtual ICollection<InventoryTracker> InventoryTrackers { get; set; } = new List<InventoryTracker>();
        public virtual ICollection<Statistic> StatisticDestinationStorages { get; set; } = new List<Statistic>();

        public virtual ICollection<Statistic> StatisticInitialStorages { get; set; } = new List<Statistic>();
    }
}

