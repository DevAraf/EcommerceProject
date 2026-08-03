using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.OrdersAndCartTable
{
    public class OrderShipment : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderShipmentId { get; set; }

        [Required]
        public long OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Orders Order { get; set; } = null!;

        [Required]
        public long OrderShipmentStatusId { get; set; }

        [ForeignKey(nameof(OrderShipmentStatusId))]
        public OrderShipmentStatus Status { get; set; } = null!;

        [MaxLength(200)]
        public string TrackingNumber { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Carrier { get; set; } = string.Empty;
    }
}
