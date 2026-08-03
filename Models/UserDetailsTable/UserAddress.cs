using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.UserDetailsTable
{
    public class UserAddress : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserAddressId { get; set; }

        [Required]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users User { get; set; } = null!;

        [Required, MaxLength(1000)]
        public string AddressLine1 { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? AddressLine2 { get; set; }

        [Required, MaxLength(200)]
        public string City { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? PostalCode { get; set; }

        [Required, MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        public bool IsDefault { get; set; } = false;
    }
}
