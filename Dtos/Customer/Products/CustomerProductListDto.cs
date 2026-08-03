namespace VendorEcommerceProject.Dtos.Customer.Products
{
    public class CustomerProductListDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }

        public string? ThumbnailImageUrl { get; set; }

        public long VendorId { get; set; }
        public string VendorName { get; set; } = null!;

        public bool InStock { get; set; }
    }
}
