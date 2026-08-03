using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using VendorEcommerceProject.Models.OrdersAndCartTable;
using VendorEcommerceProject.Models.ProductsTables;
using VendorEcommerceProject.Models.VendorsTable;

namespace VendorEcommerceProject.Models.UserDetailsTable
{
    public class Users : IdentityUser<long>
    {
        [Required, MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Navigation
        public IList<Cart> Carts { get; set; } = new List<Cart>();
        public IList<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public IList<Orders> Orders { get; set; } = new List<Orders>();
        public IList<UserAddress> Addresses { get; set; } = new List<UserAddress>();
        public IList<UserRoleAssignment> UserRoles { get; set; } = new List<UserRoleAssignment>();
        public IList<CustomersReview> CustomersReview { get; set; } = new List<CustomersReview>();
        public IList<ProductsReview> VendorProductReviews { get; set; } = new List<ProductsReview>();
        public IList<VendorReview> VendorReviews { get; set; } = new List<VendorReview>();

        // One-to-One Profiles
        public Vendor? VendorAccount { get; set; }
        public CustomerDetails? CustomerDetails { get; set; }
        public VendorDetails? VendorDetails { get; set; }
        public SuperAdminDetails? SuperAdminDetails { get; set; }
    }
}