using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagementApplication.Areas.Identity.Data;

// Add profile data for application users by adding properties to the InventoryManagementUser class
public class InventoryManagementUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }
    [PersonalData]
    public string LastName { get; set; }
    [PersonalData]
    public string EmployeeNumber { get; set; }
    [PersonalData]
    public string? RoleId { get; set; }
    [PersonalData]
    public DateTime Created {  get; set; }
    [PersonalData]
    public DateTime Updated { get; set; }

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    public virtual ICollection<Statistic> StatisticUsers { get; set; } = new List<Statistic>();
}

