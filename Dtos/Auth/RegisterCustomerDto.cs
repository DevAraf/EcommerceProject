namespace VendorEcommerceProject.Dtos.Auth
{
    public class RegisterCustomerDto
    {
        // Identity
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Customer profile
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
    }
}
