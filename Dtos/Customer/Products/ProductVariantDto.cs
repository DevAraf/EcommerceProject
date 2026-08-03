namespace VendorEcommerceProject.Dtos.Customer.Products
{
    public class ProductVariantDto
    {
        public long VariantId { get; set; }
        public string Size { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int Stock { get; set; }
    }
}
