namespace VendorEcommerceProject.Dtos.Admin.Dashboard
{
    public class AdminRecentOrderDto
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
