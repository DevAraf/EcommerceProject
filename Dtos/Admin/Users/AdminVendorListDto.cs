namespace VendorEcommerceProject.Dtos.Admin.Users
{
    public class AdminVendorListDto
    {
        public long VendorId { get; set; }
        public long UserId { get; set; }

        public string VendorName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string? ShopName { get; set; }
        public string? BusinessRegNo { get; set; }
    }
}
