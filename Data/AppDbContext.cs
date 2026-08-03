using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VendorEcommerceProject.Models;
using VendorEcommerceProject.Models.UserDetailsTable;
using VendorEcommerceProject.Models.ProductsTables;
using VendorEcommerceProject.Models.VendorsTable;
using VendorEcommerceProject.Models.OrdersAndCartTable;
using VendorEcommerceProject.Models.PaymentsTable;

public class AppDbContext
    : IdentityDbContext<Users, Role, long>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // ===================== USER =====================
    public DbSet<CustomerDetails> CustomerDetails => Set<CustomerDetails>();
    //public DbSet<VendorDetails> VendorDetails => Set<VendorDetails>();
    public DbSet<SuperAdminDetails> SuperAdminDetails => Set<SuperAdminDetails>();
    public DbSet<UserAddress> UserAddresses => Set<UserAddress>();
    public DbSet<UserRoleAssignment> UserRoleAssignments => Set<UserRoleAssignment>();
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();

    // ===================== VENDOR =====================
    //public DbSet<Vendor> Vendors => Set<Vendor>();
    //public DbSet<VendorEarning> VendorEarnings => Set<VendorEarning>();
    //public DbSet<VendorPayment> VendorPayments => Set<VendorPayment>();
    //public DbSet<VendorReview> VendorReviews => Set<VendorReview>();
    //public DbSet<VendorSetting> VendorSettings => Set<VendorSetting>();
    //public DbSet<Commission> Commissions => Set<Commission>();

    // ===================== PRODUCT =====================
    public DbSet<Products> Products => Set<Products>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<ProductStatus> ProductStatuses => Set<ProductStatus>();
    public DbSet<ProductAttribute> ProductAttributes => Set<ProductAttribute>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<ProductTag> ProductTags => Set<ProductTag>();
    public DbSet<ProductsImages> ProductImages => Set<ProductsImages>();
    public DbSet<CustomersReview> CustomersReviews => Set<CustomersReview>();
    public DbSet<ProductsReview> ProductsReviews => Set<ProductsReview>();
    public DbSet<ReviewImage> ReviewImages => Set<ReviewImage>();

    // ===================== ORDER =====================
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<CartItemVariant> CartItemVariants { get; set; } = null!;
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<Orders> Orders => Set<Orders>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
    public DbSet<OrderShipment> OrderShipments => Set<OrderShipment>();
    public DbSet<OrderShipmentStatus> OrderShipmentStatuses => Set<OrderShipmentStatus>();
    public DbSet<OrderReturn> OrderReturns => Set<OrderReturn>();
    public DbSet<OrderReturnStatus> OrderReturnStatuses => Set<OrderReturnStatus>();
    public DbSet<OrderRefund> OrderRefunds => Set<OrderRefund>();
    public DbSet<OrderRefundStatus> OrderRefundStatuses => Set<OrderRefundStatus>();

    // ===================== PAYMENT =====================
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================================================
        // COMPOSITE KEYS
        // =========================================================

        // Wishlist (User ↔ Product)
        modelBuilder.Entity<Wishlist>()
            .HasKey(w => new { w.UserId, w.ProductId });

        // User ↔ Role (M2M)
        modelBuilder.Entity<UserRoleAssignment>()
            .HasKey(x => new { x.UserId, x.RoleId });

        // =========================================================
        // ONE-TO-ONE USER PROFILES
        // =========================================================

        modelBuilder.Entity<CustomerDetails>()
            .HasKey(x => x.UserId);

        modelBuilder.Entity<VendorDetails>()
            .HasKey(x => x.UserId);

        modelBuilder.Entity<SuperAdminDetails>()
            .HasKey(x => x.UserId);

        // =========================================================
        // PRODUCT CATEGORY (SELF-REFERENCE)
        // =========================================================

        modelBuilder.Entity<ProductCategory>()
            .HasOne(pc => pc.Parent)
            .WithMany(pc => pc.Children)
            .HasForeignKey(pc => pc.ParentId)
            .OnDelete(DeleteBehavior.Restrict); // prevent category delete cycles

        // =========================================================
        // USER ↔ ROLE ASSIGNMENTS
        // =========================================================

        modelBuilder.Entity<UserRoleAssignment>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRoleAssignment>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // =========================================================
        // CART
        // =========================================================

        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithMany(u => u.Carts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull); // guest carts allowed

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany(p => p.CartItems)
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict); // protect order history

        // =========================================================
        // ORDERS
        // =========================================================

        modelBuilder.Entity<Orders>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict); // never delete orders with user

        modelBuilder.Entity<Orders>()
            .HasOne(o => o.ShippingAddress)
            .WithMany()
            .HasForeignKey(o => o.ShippingAddressId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<OrderItem>()
     .HasOne(oi => oi.Order)
     .WithMany(o => o.OrderItems)
     .HasForeignKey(oi => oi.OrderId)
     .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Orders>()
        .HasOne(o => o.PaymentMethod)
        .WithMany()
        .HasForeignKey(o => o.PaymentMethodId)
        .OnDelete(DeleteBehavior.Restrict);

        // =========================================================
        // PRODUCT VARIANTS & IMAGES
        // =========================================================

        modelBuilder.Entity<ProductVariant>()
     .HasOne(v => v.Product)
     .WithMany(p => p.Variants)
     .HasForeignKey(v => v.ProductId)
     .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<ProductsImages>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.ProductImages)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // =========================================================
        // PRODUCT REVIEWS
        // =========================================================

        // Customer review (Product + User)
        modelBuilder.Entity<CustomersReview>()
     .HasOne(cr => cr.Product)
     .WithMany(p => p.CustomerReviews)
     .HasForeignKey(cr => cr.ProductId)
     .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<CustomersReview>()
            .HasOne(cr => cr.User)
            .WithMany(u => u.CustomersReview)
            .HasForeignKey(cr => cr.UserId)
            .OnDelete(DeleteBehavior.Restrict); // break cascade path

        modelBuilder.Entity<ReviewImage>()
            .HasOne(ri => ri.Review)
            .WithMany(r => r.Images)
            .HasForeignKey(ri => ri.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);

        // Vendor moderated review
        modelBuilder.Entity<ProductsReview>()
     .HasOne(pr => pr.Product)
     .WithMany(p => p.ProductReviews)
     .HasForeignKey(pr => pr.ProductId)
     .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<ProductsReview>()
            .HasOne(pr => pr.User)
            .WithMany(u => u.VendorProductReviews)
            .HasForeignKey(pr => pr.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================================================
        // VENDOR
        // =========================================================

        modelBuilder.Entity<Vendor>()
            .HasOne(v => v.User)
            .WithOne(u => u.VendorAccount)
            .HasForeignKey<Vendor>(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VendorReview>()
            .HasOne(vr => vr.Vendor)
            .WithMany(v => v.Reviews)
            .HasForeignKey(vr => vr.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VendorReview>()
            .HasOne(vr => vr.User)
            .WithMany(u => u.VendorReviews)
            .HasForeignKey(vr => vr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VendorEarning>()
            .HasOne(ve => ve.Vendor)
            .WithMany(v => v.Earnings)
            .HasForeignKey(ve => ve.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VendorEarning>()
            .HasOne(ve => ve.Order)
            .WithMany(o => o.Earnings)
            .HasForeignKey(ve => ve.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Commission>()
            .HasOne(c => c.Vendor)
            .WithMany(v => v.Commissions)
            .HasForeignKey(c => c.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Commission>()
            .HasOne(c => c.Order)
            .WithMany(o => o.Commissions)
            .HasForeignKey(c => c.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VendorPayment>()
            .HasOne(vp => vp.Vendor)
            .WithMany(v => v.Payouts)
            .HasForeignKey(vp => vp.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================================================
        // SHIPMENT / RETURN / REFUND
        // =========================================================

        modelBuilder.Entity<OrderShipment>()
            .HasOne(s => s.Order)
            .WithMany(o => o.Shipments)
            .HasForeignKey(s => s.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderReturn>()
            .HasOne(r => r.OrderItem)
            .WithMany(oi => oi.Returns)
            .HasForeignKey(r => r.OrderItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderRefund>()
            .HasOne(rf => rf.Return)
            .WithMany(r => r.Refunds)
            .HasForeignKey(rf => rf.OrderReturnId)
            .OnDelete(DeleteBehavior.Cascade);

        // =========================================================
        // PAYMENT
        // =========================================================

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Order)
            .WithMany(o => o.Payments)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Method)
            .WithMany(m => m.Payments)
            .HasForeignKey(p => p.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================================================
        // UNIQUE BUSINESS RULES
        // =========================================================

        modelBuilder.Entity<Products>()
            .HasIndex(p => p.Sku)
            .IsUnique();

        modelBuilder.Entity<Coupon>()
            .HasIndex(c => c.Code)
            .IsUnique();


        //cart item variants (cart items ↔ product variants)


        modelBuilder.Entity<CartItemVariant>()
    .HasKey(x => new { x.CartItemId, x.ProductVariantId });

        modelBuilder.Entity<CartItemVariant>()
            .HasOne(x => x.CartItem)
            .WithMany(c => c.CartItemVariants)
            .HasForeignKey(x => x.CartItemId);

        modelBuilder.Entity<CartItemVariant>()
            .HasOne(x => x.ProductVariant)
            .WithMany()
            .HasForeignKey(x => x.ProductVariantId);



        // -------------------------------
        // SEED ROLES
        // -------------------------------
        modelBuilder.Entity<Role>().HasData(
           new Role
           {
               Id = 1,
               Name = "SuperAdmin",
               NormalizedName = "SUPERADMIN",
               ConcurrencyStamp = "ROLE_SUPERADMIN_STATIC"
           },
           new Role
           {
               Id = 2,
               Name = "Admin",
               NormalizedName = "ADMIN",
               ConcurrencyStamp = "ROLE_ADMIN_STATIC"
           },
           new Role
           {
               Id = 3,
               Name = "Vendor",
               NormalizedName = "VENDOR",
               ConcurrencyStamp = "ROLE_VENDOR_STATIC"
           },
           new Role
           {
               Id = 4,
               Name = "Customer",
               NormalizedName = "CUSTOMER",
               ConcurrencyStamp = "ROLE_CUSTOMER_STATIC"
           }
       );

        // -------------------------------
        // SEED PRODUCT STATUSES
        // -------------------------------
        modelBuilder.Entity<ProductStatus>().HasData(
            new ProductStatus
            {
                ProductStatusId = 1,
                Name = "Pending",
                DisplayName = "Pending Approval"
            },
            new ProductStatus
            {
                ProductStatusId = 2,
                Name = "Approved",
                DisplayName = "Approved"
            },
            new ProductStatus
            {
                ProductStatusId = 3,
                Name = "Rejected",
                DisplayName = "Rejected"
            },
            new ProductStatus
            {
                ProductStatusId = 4,
                Name = "Blocked",
                DisplayName = "Blocked by Admin"
            }
        );
        modelBuilder.Entity<OrderStatus>().HasData(
            
            new OrderStatus { OrderStatusId = 1, Name = "Pending" },
            new OrderStatus { OrderStatusId = 2, Name = "Processing" },
            new OrderStatus { OrderStatusId = 3, Name = "Shipped" },
            new OrderStatus { OrderStatusId = 4, Name = "Delivered" },
            new OrderStatus { OrderStatusId = 5, Name = "Cancelled" }
        );


        //// -------------------------------
        //// SEED 3 SUPER ADMINS
        //// -------------------------------
        //var hasher = new PasswordHasher<Users>();

        //var admin1 = new Users
        //{
        //    Id = 1,
        //    UserName = "superadmin1",
        //    NormalizedUserName = "SUPERADMIN1",
        //    Email = "admin1@system.com",
        //    NormalizedEmail = "ADMIN1@SYSTEM.COM",
        //    EmailConfirmed = true,
        //    FullName = "System Super Admin 1",
        //    IsActive = true,
        //    PasswordHash = hasher.HashPassword(null!, "Admin@123")
        //};

        //var admin2 = new Users
        //{
        //    Id = 2,
        //    UserName = "superadmin2",
        //    NormalizedUserName = "SUPERADMIN2",
        //    Email = "admin2@system.com",
        //    NormalizedEmail = "ADMIN2@SYSTEM.COM",
        //    EmailConfirmed = true,
        //    FullName = "System Super Admin 2",
        //    IsActive = true,
        //    PasswordHash = hasher.HashPassword(null!, "Admin@123")
        //};

        //var admin3 = new Users
        //{
        //    Id = 3,
        //    UserName = "superadmin3",
        //    NormalizedUserName = "SUPERADMIN3",
        //    Email = "admin3@system.com",
        //    NormalizedEmail = "ADMIN3@SYSTEM.COM",
        //    EmailConfirmed = true,
        //    FullName = "System Super Admin 3",
        //    IsActive = true,
        //    PasswordHash = hasher.HashPassword(null!, "Admin@123")
        //};

        //modelBuilder.Entity<Users>().HasData(admin1, admin2, admin3);

        //modelBuilder.Entity<UserRoleAssignment>().HasData(
        //    new UserRoleAssignment { UserId = 1, RoleId = 1 },
        //    new UserRoleAssignment { UserId = 2, RoleId = 1 },
        //    new UserRoleAssignment { UserId = 3, RoleId = 1 }
        //);
    }
}

