using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendorEcommerceProject.Models.ProductsTables;

namespace VendorEcommerceProject.Models.OrdersAndCartTable
{
    public class OrderItem : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderItemId { get; set; }

        [Required]
        public long OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Orders Order { get; set; } = null!;

        [Required]
        public long ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Products Product { get; set; } = null!;

        public long? ProductVariantId { get; set; }

        [ForeignKey(nameof(ProductVariantId))]
        public ProductVariant? Variant { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public IList<OrderReturn> Returns { get; set; } = new List<OrderReturn>();
    }
}
