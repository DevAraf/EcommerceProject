using System.ComponentModel.DataAnnotations;
using VendorEcommerceProject.Models.OrdersAndCartTable;

namespace VendorEcommerceProject.Dtos.Admin.Coupons
{
    public class AdminCouponUpdateDto
    {
        [Required]
        public long CouponId { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        //public string Type { get; set; } = "fixed";
        public CouponType Type { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        [Required]
        public int UsageLimit { get; set; }
    }
}
