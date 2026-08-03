namespace VendorEcommerceProject.Dtos.Admin.ProductAttributes
{
    public class AdminProductAttributeListDto
    {
        public long ProductAttributeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int SortOrder { get; set; }
    }
}
