namespace VendorEcommerceProject.Dtos.Customer.Profile
{
    public class CustomerProfileDto
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        public List<CustomerAddressDto> Addresses { get; set; } = new List<CustomerAddressDto>();
    }
}
