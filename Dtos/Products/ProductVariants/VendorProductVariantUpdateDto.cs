using System.ComponentModel.DataAnnotations;

namespace VendorEcommerceProject.Dtos.Vendor.ProductVariants
{
    public class VendorProductVariantUpdateDto
    {
        [Required]
        public long ProductVariantId { get; set; }

        [Required, MaxLength(200)]
        public string Value { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
