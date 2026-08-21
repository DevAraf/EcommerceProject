using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendorEcommerceProject.Models.OrdersAndCartTable;

namespace VendorEcommerceProject.Models.ProductsTables
{
    public class ProductVariant : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductVariantId { get; set; }

        [Required]
        public long ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Products Product { get; set; } = null!;

        [Required]
        public long ProductAttributeId { get; set; }

        [ForeignKey(nameof(ProductAttributeId))]
        public ProductAttribute Attribute { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Value { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }


        public long? ParentVariantId { get; set; }

        [ForeignKey(nameof(ParentVariantId))]
        public ProductVariant? ParentVariant { get; set; }

        public IList<ProductVariant> ChildVariants { get; set; }
            = new List<ProductVariant>();

        public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public IList<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
