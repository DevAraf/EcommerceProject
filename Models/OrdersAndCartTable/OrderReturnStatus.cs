using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.OrdersAndCartTable
{
    public class OrderReturnStatus : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderReturnStatusId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public IList<OrderReturn> Returns { get; set; } = new List<OrderReturn>();
    }
}
