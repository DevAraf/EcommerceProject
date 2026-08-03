namespace VendorEcommerceProject.Dtos.UserRoles
{
    public class ChangeRoleDto
    {
        public long UserId { get; set; }
        public string OldRole { get; set; } = string.Empty;
        public string NewRole { get; set; } = string.Empty;
    }
}
