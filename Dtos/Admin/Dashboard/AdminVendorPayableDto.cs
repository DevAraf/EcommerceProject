namespace VendorEcommerceProject.Dtos.Admin.Dashboard
{
    public class AdminVendorPayableDto
    {
        public long VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public decimal PayableAmount { get; set; }
    }
}
