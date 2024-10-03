namespace InventoryManagementApplication.Models
{
    public class UserStatisticsViewModel
    {
        public string? EmployeeNumber { get; set; }
        public int TotalMovements { get; set; }
        public int TotalQuantity { get; set; }
        public List<Statistic> RecentMovements { get; set; } = new List<Statistic>(); 
    }
}
