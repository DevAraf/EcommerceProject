using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendorEcommerceProject.Models.UserDetailsTable;

namespace VendorEcommerceProject.Models.OrdersAndCartTable
{
    public class Cart : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CartId { get; set; }

        public long? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users? User { get; set; }

        [MaxLength(250)]
        public string? SessionId { get; set; }

        public IList<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
