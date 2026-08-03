namespace VendorEcommerceProject.Dtos.Customer.Cart
{
    public class CustomerCartDto
    {
        public long? UserId { get; set; }
        public List<CartVendorDto> Vendors { get; set; } = new List<CartVendorDto>();
        public decimal GrandTotal { get; set; }
    }
}
