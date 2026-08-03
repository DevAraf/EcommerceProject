using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.ProductsTables
{
    public class ProductsImages : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductImageId { get; set; }

        [Required]
        public long ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Products Product { get; set; } = null!;

        [Required, MaxLength(1000)]
        public string ImageUrl { get; set; } = string.Empty;

        [MaxLength(260)]
        public string? FileName { get; set; }

        [MaxLength(100)]
        public string? ImageFileType { get; set; }

        public long? FileSize { get; set; }
    }
}
