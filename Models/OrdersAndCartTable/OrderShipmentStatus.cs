using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.OrdersAndCartTable
{
    public class OrderShipmentStatus : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderShipmentStatusId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public IList<OrderShipment> Shipments { get; set; } = new List<OrderShipment>();
    }
}
