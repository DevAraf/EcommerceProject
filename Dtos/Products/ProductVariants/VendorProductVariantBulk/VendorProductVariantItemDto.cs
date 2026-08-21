namespace VendorEcommerceProject.Dtos.Vendor.ProductVariants.VendorProductVariantBulk
{
    public class VendorProductVariantItemDto
    {
        public long ProductAttributeId { get; set; }

        public long? ParentVariantId { get; set; }
        public string Value { get; set; } = string.Empty;
        public decimal AdditionalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
