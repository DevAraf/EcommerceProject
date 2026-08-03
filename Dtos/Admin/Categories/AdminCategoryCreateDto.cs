namespace VendorEcommerceProject.Dtos.Admin.Categories
{
    public class AdminCategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public long? ParentId { get; set; } // null = root category
    }

}
