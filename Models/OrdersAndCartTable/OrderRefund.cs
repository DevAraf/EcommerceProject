using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.OrdersAndCartTable
{
    public class OrderRefund : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderRefundId { get; set; }

        [Required]
        public long OrderReturnId { get; set; }

        [ForeignKey(nameof(OrderReturnId))]
        public OrderReturn Return { get; set; } = null!;

        [Required]
        public long OrderRefundStatusId { get; set; }

        [ForeignKey(nameof(OrderRefundStatusId))]
        public OrderRefundStatus Status { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
