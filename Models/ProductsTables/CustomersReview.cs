using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendorEcommerceProject.Models.UserDetailsTable;

namespace VendorEcommerceProject.Models.ProductsTables
{
    public class CustomersReview : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CustomersReviewId { get; set; }

        [Required]
        public long ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Products Product { get; set; } = null!;

        [Required]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users User { get; set; } = null!;

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(5000)]
        public string Comment { get; set; } = string.Empty;

        public IList<ReviewImage> Images { get; set; } = new List<ReviewImage>();
    }
}
