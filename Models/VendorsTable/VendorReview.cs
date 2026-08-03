using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendorEcommerceProject.Models.UserDetailsTable;

namespace VendorEcommerceProject.Models.VendorsTable
{
    public class VendorReview : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VendorReviewId { get; set; }

        [Required]
        public long VendorId { get; set; }

        [ForeignKey(nameof(VendorId))]
        public Vendor Vendor { get; set; } = null!;

        [Required]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users User { get; set; } = null!;

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(2000)]
        public string Comment { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? ImageUrl { get; set; }
    }
}
