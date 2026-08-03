using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.VendorsTable
{
    public class VendorPayment : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VendorPaymentId { get; set; }

        [Required]
        public long VendorId { get; set; }

        [ForeignKey(nameof(VendorId))]
        public Vendor Vendor { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}
