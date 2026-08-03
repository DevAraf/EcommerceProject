using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendorEcommerceProject.Models.ProductsTables;
using VendorEcommerceProject.Models.UserDetailsTable;

namespace VendorEcommerceProject.Models.VendorsTable
{
    public class Vendor : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VendorId { get; set; }

        [Required]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users User { get; set; } = null!;

        [Required, MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        [Column(TypeName = "nvarchar(2000)")]
        public string Description { get; set; } = string.Empty;

        [MaxLength(1000)]
        [Column(TypeName = "nvarchar(1000)")]
        public string? ImageUrl { get; set; }

        // Navigation
        public IList<Products> Products { get; set; } = new List<Products>();
        public IList<VendorEarning> Earnings { get; set; } = new List<VendorEarning>();
        public IList<VendorPayment> Payouts { get; set; } = new List<VendorPayment>();
        public IList<Commission> Commissions { get; set; } = new List<Commission>();
        public IList<VendorReview> Reviews { get; set; } = new List<VendorReview>();
        public IList<VendorSetting> Settings { get; set; } = new List<VendorSetting>();
    }

}
