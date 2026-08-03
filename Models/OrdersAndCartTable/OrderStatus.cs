using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.OrdersAndCartTable
{
    public class OrderStatus : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderStatusId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public IList<Orders> Orders { get; set; } = new List<Orders>();
    }
}
