namespace VendorEcommerceProject.Dtos.Customer.Cart
{
    public class CartVendorDto
    {
        public long VendorId { get; set; }
        public string VendorName { get; set; }

        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public decimal SubTotal { get; set; }
    }
}
