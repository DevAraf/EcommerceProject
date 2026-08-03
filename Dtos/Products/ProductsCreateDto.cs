using System.ComponentModel.DataAnnotations;

namespace VendorEcommerceProject.Dtos.Products
{
    public class ProductCreateDto
    {
        [Required]
        public long CategoryId { get; set; }

        [Required, MaxLength(200)]
        public string ProductsName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required, MaxLength(100)]
        public string Sku { get; set; } = string.Empty;

        // Multiple images upload
        public IList<IFormFile>? Images { get; set; }
    }
}
