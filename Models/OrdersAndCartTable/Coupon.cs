using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.OrdersAndCartTable
{
    public class Coupon : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CouponId { get; set; }

        [Required, MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }

        //[Required, MaxLength(20)]
        //blic string Type { get; set; } = "fixed";
        [Required]
        public CouponType Type { get; set; } = CouponType.Fixed;


        //[Required, MaxLength(20)]
        //public string Type { get; set; } = "fixed";

        public DateTime ValidFrom { get; set; } = DateTime.UtcNow;
        public DateTime ValidTo { get; set; } = DateTime.UtcNow.AddMonths(1);

        [Range(0, int.MaxValue)]
        public int UsageLimit { get; set; }

        public IList<Orders> Orders { get; set; } = new List<Orders>();
    }

    public enum CouponType
    {
        Fixed = 1,
        Percentage = 2
    }

}
