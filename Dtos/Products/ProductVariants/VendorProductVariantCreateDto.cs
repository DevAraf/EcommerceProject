using System.ComponentModel.DataAnnotations;

namespace VendorEcommerceProject.Dtos.Vendor.ProductVariants
{
    public class VendorProductVariantCreateDto
    {
        [Required]
        public long ProductId { get; set; }

        [Required]
        public long ProductAttributeId { get; set; }

        [Required, MaxLength(200)]
        public string Value { get; set; } = string.Empty;
        // e.g. Red, Blue, XL


        public long? ParentVariantId { get; set; }
        [Required]
        public decimal Price { get; set; }
        // price difference from base product

        [Required]
        public int Quantity { get; set; }
    }
}
