namespace VendorEcommerceProject.Dtos.Admin.ProductsStatusDtos
{
    public class AdminProductListDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? FirstImageUrl { get; set; }
    }

}
