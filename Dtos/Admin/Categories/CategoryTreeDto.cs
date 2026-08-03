namespace VendorEcommerceProject.Dtos.Admin.Categories
{
    public class CategoryTreeDto
    {
        public long ProductCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CategoryTreeDto> Children { get; set; } = new();
    }

}
