namespace VendorEcommerceProject.Dtos.Products
{
    public class ProductDetailsDto
    {
        public long ProductId { get; set; }

        public string ProductsName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Sku { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string StatusName { get; set; } = string.Empty;

        public IList<ProductImageDto> Images { get; set; }
            = new List<ProductImageDto>();
    }
}
