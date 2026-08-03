using System.Collections.Generic;

namespace VendorEcommerceProject.Dtos.Customer.Dashboard
{
    // ============================
    // Product DTO for Cart, Wishlist, Order Items
    // ============================
    public class DashboardProductDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public long VendorId { get; set; }
        public string VendorName { get; set; } = null!;
        public bool InStock { get; set; }
        public string? ThumbnailImageUrl { get; set; }
        public int Quantity { get; set; } // Cart / Order quantity
    }

    // ============================
    // Recent Orders DTO
    // ============================
    public class DashboardOrderDto
    {
        public long OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public List<DashboardProductDto> Items { get; set; } = new();
    }

    // ============================
    // Customer Address DTO
    // ============================
    public class DashboardAddressDto
    {
       
        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        
        public string PostalCode { get; set; } = null!;
        public bool IsDefault { get; set; }
    }

    // ============================
    // Combined Dashboard DTO
    // ============================
    public class CustomerDashboardDto
    {
        public List<DashboardProductDto> Cart { get; set; } = new();
        public List<DashboardOrderDto> RecentOrders { get; set; } = new();
        public List<DashboardProductDto> Wishlist { get; set; } = new();
        public List<DashboardAddressDto> Addresses { get; set; } = new();
    }
}
