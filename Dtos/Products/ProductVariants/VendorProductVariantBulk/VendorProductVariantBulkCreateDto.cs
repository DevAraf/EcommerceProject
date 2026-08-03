namespace VendorEcommerceProject.Dtos.Vendor.ProductVariants.VendorProductVariantBulk
{
    public class VendorProductVariantBulkCreateDto
    {
        public long ProductId { get; set; }
        public IList<VendorProductVariantItemDto> Variants { get; set; } = new List<VendorProductVariantItemDto>();
    }
}
