namespace VendorEcommerceProject.Dtos.Admin.ProductsStatusDtos
{
    public class AdminProductStatusUpdateDto
    {
        public long ProductId { get; set; }
        public long ProductStatusId { get; set; } // Pending / Approved / Rejected / Blocked
    }

}
