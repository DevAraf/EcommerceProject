namespace VendorEcommerceProject.Dtos.Auth
{
    public class RegisterAdminDto
    {
        // Identity
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Admin notes
        public string? Notes { get; set; }
    }
}
