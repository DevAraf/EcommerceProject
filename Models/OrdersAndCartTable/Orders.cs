using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendorEcommerceProject.Models.PaymentsTable;
using VendorEcommerceProject.Models.UserDetailsTable;
using VendorEcommerceProject.Models.VendorsTable;

namespace VendorEcommerceProject.Models.OrdersAndCartTable
{
    public class Orders : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderId { get; set; }

        [Required]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users User { get; set; } = null!;

        [Required]
        public long OrderStatusId { get; set; }

        [ForeignKey(nameof(OrderStatusId))]
        public OrderStatus Status { get; set; } = null!;

        public long? ShippingAddressId { get; set; }

        [ForeignKey(nameof(ShippingAddressId))]
        public UserAddress? ShippingAddress { get; set; }

        public long? CouponId { get; set; }

        [ForeignKey(nameof(CouponId))]
        public Coupon? Coupon { get; set; }

        public long PaymentMethodId { get; set; }

        [ForeignKey(nameof(PaymentMethodId))]
        public PaymentMethod PaymentMethod { get; set; } = null!;


        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public IList<OrderShipment> Shipments { get; set; } = new List<OrderShipment>();
        public IList<Payment> Payments { get; set; } = new List<Payment>();
        public IList<Commission> Commissions { get; set; } = new List<Commission>();
        public IList<VendorEarning> Earnings { get; set; } = new List<VendorEarning>();
    }
}
