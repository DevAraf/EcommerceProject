using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendorEcommerceProject.Models.OrdersAndCartTable;

namespace VendorEcommerceProject.Models.PaymentsTable
{
    public class Payment : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PaymentId { get; set; }

        [Required]
        public long OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Orders Order { get; set; } = null!;

        [Required]
        public long PaymentMethodId { get; set; }

        [ForeignKey(nameof(PaymentMethodId))]
        public PaymentMethod Method { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required, MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Status { get; set; } = string.Empty;
    }
}
