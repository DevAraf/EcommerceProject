namespace VendorEcommerceProject.Dtos.Admin.Dashboard
{
    public class AdminDashboardSummaryDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCommission { get; set; }
        public int PendingProducts { get; set; }
        public decimal PendingVendorPayable { get; set; }
    }
}
