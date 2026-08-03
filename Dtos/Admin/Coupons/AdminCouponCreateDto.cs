using System.ComponentModel.DataAnnotations;
using VendorEcommerceProject.Models.OrdersAndCartTable;

namespace VendorEcommerceProject.Dtos.Admin.Coupons
{
    public class AdminCouponCreateDto
    {
        [Required, MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        [Required]
        public decimal Discount { get; set; }

        // "fixed" or "percentage"
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
