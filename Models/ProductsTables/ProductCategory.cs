using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.ProductsTables
{
    public class ProductCategory : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductCategoryId { get; set; }

        public long? ParentId { get; set; }

        [ForeignKey(nameof(ParentId))]
        public ProductCategory? Parent { get; set; }

        public ICollection<ProductCategory> Children { get; set; } = new List<ProductCategory>();

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Slug { get; set; }

        public IList<Products> Products { get; set; } = new List<Products>();
    }

}
