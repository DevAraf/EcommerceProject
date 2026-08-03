using VendorEcommerceProject.Models.OrdersAndCartTable;

namespace VendorEcommerceProject.Dtos.Admin.Coupons
{
    public class AdminCouponListDto
    {
        public long CouponId { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        //public string Type { get; set; } = string.Empty;
        public CouponType Type { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int UsageLimit { get; set; }
    }
}
