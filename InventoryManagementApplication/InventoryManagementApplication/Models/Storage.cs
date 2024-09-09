namespace InventoryManagementApplication.Models
{
    public class Storage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MaxCapacity { get; set; }
        public int? CurrentStock { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public virtual ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();
    }
}
