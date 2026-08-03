using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.OrdersAndCartTable
{
    public class OrderRefundStatus : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderRefundStatusId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Navigation
        public IList<OrderRefund> Refunds { get; set; } = new List<OrderRefund>();
    }
}
