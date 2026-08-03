namespace VendorEcommerceProject.Dtos.Customer.Orders
{
    public class CustomerOrderDetailsDto
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
