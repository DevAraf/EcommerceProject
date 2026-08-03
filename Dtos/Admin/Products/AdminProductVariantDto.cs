namespace VendorEcommerceProject.Dtos.Admin.Products
{
    public class AdminProductVariantDto
    {
        public long ProductVariantId { get; set; }
        public string AttributeName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
