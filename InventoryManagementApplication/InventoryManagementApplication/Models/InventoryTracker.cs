namespace InventoryManagementApplication.Models
{
    public class InventoryTracker
    {
        public int Id { get; set; }
        public int StorageId { get; set; }
        public int ProductId { get; set; }
        public int ProductStock { get; set; }
        public virtual Storage? Storage { get; set; }
        public virtual Product? Product { get; set; }
    }
}
