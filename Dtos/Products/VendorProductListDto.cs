namespace VendorEcommerceProject.Dtos.Products
{
    public class ProductListDto
    {
        public long ProductId { get; set; }

        public string ProductsName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string StatusName { get; set; } = string.Empty;

        // ✅ FIRST IMAGE (Primary / Thumbnail)
        public string? FirstImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
