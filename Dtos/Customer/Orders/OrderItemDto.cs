namespace VendorEcommerceProject.Dtos.Customer.Orders
{
    public class OrderItemDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
