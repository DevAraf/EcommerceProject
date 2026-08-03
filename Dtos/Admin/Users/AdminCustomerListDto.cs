namespace VendorEcommerceProject.Dtos.Admin.Users
{
    public class AdminCustomerListDto
    {
        public long UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
    }
}
