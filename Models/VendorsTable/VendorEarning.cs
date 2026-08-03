using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendorEcommerceProject.Models.OrdersAndCartTable;

namespace VendorEcommerceProject.Models.VendorsTable
{
    public class VendorEarning : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VendorEarningId { get; set; }

        [Required]
        public long VendorId { get; set; }

        [ForeignKey(nameof(VendorId))]
        public Vendor Vendor { get; set; } = null!;

        [Required]
        public long OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Orders Order { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required, MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Status { get; set; } = "pending";
    }
}
