using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.UserDetailsTable
{
    public class SuperAdminDetails : BaseEntity
    {
        [Key]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users User { get; set; } = null!;

        [MaxLength(2000)]
        public string? Notes { get; set; }
    }
}
