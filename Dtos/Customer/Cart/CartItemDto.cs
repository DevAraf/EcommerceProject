namespace VendorEcommerceProject.Dtos.Customer.Cart
{
    public class CartItemDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public long? VariantId { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
    }
}
