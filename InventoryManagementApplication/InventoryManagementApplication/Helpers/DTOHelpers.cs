using InventoryManagementApplication.Areas.Identity.Data;

namespace InventoryManagementApplication.Helpers
{
    public class DTOHelpers
    {
        public static DTO.RolesDTO SetDTO(InventoryManagementUser? user, List<string?> currentRoles, string? addRole, bool resetPassword)
        {
            var dto = new DTO.RolesDTO()
            {
                User = user,
                CurrentRoles = currentRoles,
                AddRole = addRole,
                ResetPassword = resetPassword,
                IsDeleted = user.IsDeleted != null ? user.IsDeleted.Value : false
            };

            return dto;
        }
    }
}
