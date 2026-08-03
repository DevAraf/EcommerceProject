namespace VendorEcommerceProject.Dtos.Customer.Orders
{
    public class CustomerOrderListDto
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
