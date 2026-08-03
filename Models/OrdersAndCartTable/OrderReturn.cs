using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.OrdersAndCartTable
{
    public class OrderReturn : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderReturnId { get; set; }

        [Required]
        public long OrderItemId { get; set; }

        [ForeignKey(nameof(OrderItemId))]
        public OrderItem OrderItem { get; set; } = null!;

        [Required]
        public long OrderReturnStatusId { get; set; }

        [ForeignKey(nameof(OrderReturnStatusId))]
        public OrderReturnStatus Status { get; set; } = null!;

        [Required, MaxLength(2000)]
        public string Reason { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        public IList<OrderRefund> Refunds { get; set; } = new List<OrderRefund>();
    }
}
