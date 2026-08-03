namespace VendorEcommerceProject.Dtos.Customer.Profile
{
    public class CustomerAddressDto
    {
        public long AddressId { get; set; }

        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }

        public string Division { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string PostalCode { get; set; }

        public bool IsDefault { get; set; }
    }
}
