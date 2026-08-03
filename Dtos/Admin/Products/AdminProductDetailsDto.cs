namespace VendorEcommerceProject.Dtos.Admin.Products
{
    public class AdminProductDetailsDto
    {
        public long ProductId { get; set; }

        public string ProductsName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string StatusName { get; set; } = string.Empty;

        // Vendor info
        public long VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;

        // Category
        public string CategoryName { get; set; } = string.Empty;

        // Collections
        public IList<AdminProductImageDto> Images { get; set; } = new List<AdminProductImageDto>();
        public IList<AdminProductVariantDto> Variants { get; set; } = new List<AdminProductVariantDto>();
        public IList<AdminProductTagDto> Tags { get; set; } = new List<AdminProductTagDto>();
    }
}
