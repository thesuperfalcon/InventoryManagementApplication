using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using InventoryManagementApplication.Models;
using Microsoft.AspNetCore.Identity;
namespace InventoryManagementApplication.Areas.Identity.Data;
#nullable enable

public class InventoryManagementUser : IdentityUser
{
	[JsonPropertyName("id")]
	public override string Id { get; set; }

	[JsonPropertyName("firstName")]
    [PersonalData]
    public string FirstName { get; set; }

	[JsonPropertyName("lastName")]
	[PersonalData]
    public string LastName { get; set; }

	[JsonPropertyName("employeeNumber")]
	[PersonalData]
    public string EmployeeNumber { get; set; }

	[JsonPropertyName("roleId")]
	[PersonalData]
    public string? RoleId { get; set; }

	[JsonPropertyName("created")]
	[PersonalData]
    public DateTime Created {  get; set; } = DateTime.Now;

    [JsonPropertyName("updated")]
	[PersonalData]
    public DateTime Updated { get; set; }

    [JsonPropertyName("profilePic")]
    [PersonalData]
    public string? ProfilePic { get; set; }

    [JsonPropertyName("isDeleted")]
    public bool? IsDeleted { get; set; } = false;

    [JsonPropertyName("userName")]
    public override string UserName { get; set; }
}

