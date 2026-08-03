using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.ProductsTables
{
    public class ReviewImage : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReviewImageId { get; set; }

        [Required]
        public long ReviewId { get; set; }

        [ForeignKey(nameof(ReviewId))]
        public CustomersReview Review { get; set; } = null!;

        [Required, MaxLength(1000)]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
