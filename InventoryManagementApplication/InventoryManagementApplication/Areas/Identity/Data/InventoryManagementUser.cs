using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public int EmployeeNumber { get; set; }
    [PersonalData]
    public string? RoleId { get; set; }
    [PersonalData]
    public DateTime Created {  get; set; }
    [PersonalData]
    public DateTime Updated { get; set; }
}

