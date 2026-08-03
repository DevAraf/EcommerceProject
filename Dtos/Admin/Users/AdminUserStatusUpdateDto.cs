namespace VendorEcommerceProject.Dtos.Admin.Users
{
    public class AdminUserStatusUpdateDto
    {
        public long UserId { get; set; }
        public bool IsActive { get; set; } // true = unblock, false = block
    }
}
