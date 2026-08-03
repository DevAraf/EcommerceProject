using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.UserDetailsTable
{
    public class VendorDetails : BaseEntity
    {
        [Key]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users User { get; set; } = null!;

        [MaxLength(200)]
        public string? ShopName { get; set; }

        [MaxLength(100)]
        public string? BusinessRegNo { get; set; }

        [MaxLength(100)]
        public string? TaxId { get; set; }
    }
}
