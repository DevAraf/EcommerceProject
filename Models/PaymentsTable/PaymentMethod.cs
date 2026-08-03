using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.PaymentsTable
{
    public class PaymentMethod : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PaymentMethodId { get; set; }

        [Required, MaxLength(150)]
        [Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; } = true;

        // Navigation
        public IList<Payment> Payments { get; set; } = new List<Payment>();
    }
}
