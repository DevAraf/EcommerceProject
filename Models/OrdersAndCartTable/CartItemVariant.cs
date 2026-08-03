using System.ComponentModel.DataAnnotations.Schema;
using VendorEcommerceProject.Models.ProductsTables;

namespace VendorEcommerceProject.Models.OrdersAndCartTable
{
    public class CartItemVariant
    {
        public long CartItemId { get; set; }

        [ForeignKey(nameof(CartItemId))]
        public CartItem CartItem { get; set; } = null!;

        public long ProductVariantId { get; set; }

        [ForeignKey(nameof(ProductVariantId))]
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}
