namespace VendorEcommerceProject.Dtos.Customer.Products
{
    public class CustomerProductDetailsDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public List<string> Images { get; set; } = new();

        public long VendorId { get; set; }
        public string VendorName { get; set; } = null!;

        public bool InStock { get; set; }
        public List<ProductVariantDto> Variants { get; set; } = new();
    }
}
