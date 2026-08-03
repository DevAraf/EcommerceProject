namespace VendorEcommerceProject.Dtos.Admin.Categories
{
    public class AdminCategoryUpdateDto
    {
        public long ProductCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public long? ParentId { get; set; }
    }

}
