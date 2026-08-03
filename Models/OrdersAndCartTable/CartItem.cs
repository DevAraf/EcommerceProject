using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendorEcommerceProject.Models.ProductsTables;

namespace VendorEcommerceProject.Models.OrdersAndCartTable
{
    public class CartItem : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CartItemId { get; set; }

        [Required]
        public long CartId { get; set; }

        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; } = null!;

        [Required]
        public long ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Products Product { get; set; } = null!;

        public long? ProductVariantId { get; set; }

        [ForeignKey(nameof(ProductVariantId))]
        public ProductVariant? Variant { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        public ICollection<CartItemVariant> CartItemVariants { get; set; } = new List<CartItemVariant>();
 
    }
}
