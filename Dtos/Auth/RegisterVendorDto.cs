namespace VendorEcommerceProject.Dtos.Auth
{
    public class RegisterVendorDto
    {
        // Identity
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Vendor details
        public string ShopName { get; set; } = string.Empty;
        public string? BusinessRegNo { get; set; }
        public string? TaxId { get; set; }

        // Vendor profile
        public string VendorDisplayName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
