using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.UserDetailsTable
{
    public class CustomerDetails : BaseEntity
    {
        [Key]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users User { get; set; } = null!;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; }
    }
}
