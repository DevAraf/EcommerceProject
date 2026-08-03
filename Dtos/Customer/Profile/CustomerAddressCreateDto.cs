namespace EcommerceProject.Dtos.Customer.Profile
{
    public class CustomerAddressCreateDto
    {
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }

        public string City { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string Country { get; set; } = string.Empty;

        public bool IsDefault { get; set; }
    }
}
