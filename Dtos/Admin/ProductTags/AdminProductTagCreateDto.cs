using System.ComponentModel.DataAnnotations;

namespace VendorEcommerceProject.Dtos.Admin.ProductTags
{
    public class AdminProductTagCreateDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
