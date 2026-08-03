namespace VendorEcommerceProject.Dtos.Customer.Cart
{
    public class AddCartItemRequestDto
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }

        // multiple variants
        public List<ProductVariantSelectionDto> Variants { get; set; } = new();
    }

    public class ProductVariantSelectionDto
    {
        public long VariantId { get; set; }
        public long AttributeId { get; set; } // size, color, etc.
    }
}
