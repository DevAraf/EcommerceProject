namespace VendorEcommerceProject.Dtos.Admin.ProductsStatusDtos
{
    public class AdminProductDetailsDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Sku { get; set; } = string.Empty;

        public string VendorName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;

        public long ProductStatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }

}
