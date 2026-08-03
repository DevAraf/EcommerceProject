using System.ComponentModel.DataAnnotations;

namespace VendorEcommerceProject.Dtos.Admin.ProductAttributes
{
    public class AdminProductAttributeUpdateDto
    {
        [Required]
        public long ProductAttributeId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public int SortOrder { get; set; }
    }
}
