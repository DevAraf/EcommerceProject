using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendorEcommerceProject.Dtos.Customer.Dashboard;
using VendorEcommerceProject.Models.OrdersAndCartTable;
using VendorEcommerceProject.Models.ProductsTables;
using VendorEcommerceProject.Models.UserDetailsTable;

[ApiController]
[Route("api/customer/dashboard")]
[Authorize(Roles = "Customer")]
public class CustomerDashboardController : ControllerBase
{
    private readonly AppDbContext _db;

    public CustomerDashboardController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // ======================
        // 1️⃣ Cart Items
        // ======================
        var cartItems = await _db.CartItems
    .Where(ci => ci.Cart.UserId == userId)
    .Include(ci => ci.Product)
        .ThenInclude(p => p.ProductImages)
    .Include(ci => ci.CartItemVariants)
        .ThenInclude(civ => civ.ProductVariant)
    .Select(ci => new DashboardProductDto
    {
        ProductId = ci.ProductId,
        ProductName = ci.Product.ProductsName,
        Price = ci.CartItemVariants
            .Select(v => v.ProductVariant.Price)
            .FirstOrDefault(),
        InStock = ci.Product.Quantity > 0,
        ThumbnailImageUrl = ci.Product.ProductImages
            .OrderBy(pi => pi.ProductImageId)
            .Select(pi => pi.ImageUrl)
            .FirstOrDefault(),
        Quantity = ci.Quantity
    })
    .ToListAsync();

        // ======================
        // 2️⃣ Recent Orders
        // ======================
        var recentOrders = await _db.Orders
     .Where(o => o.UserId == userId)
     .OrderByDescending(o => o.CreatedAt)
     .Take(5)
     .Include(o => o.OrderItems)
         .ThenInclude(oi => oi.Product)
             .ThenInclude(p => p.ProductImages)
     .Include(o => o.Status)
     .Select(o => new DashboardOrderDto
     {
         OrderId = o.OrderId,
         TotalAmount = o.OrderItems.Sum(oi => oi.Quantity * oi.Price),
         Status = o.Status.Name,
         Items = o.OrderItems.Select(oi => new DashboardProductDto
         {
             ProductId = oi.ProductId,
             ProductName = oi.Product.ProductsName,
             Price = oi.Price,
             InStock = oi.Product.Quantity > 0,
             ThumbnailImageUrl = oi.Product.ProductImages
                 .OrderBy(pi => pi.ProductImageId)
                 .Select(pi => pi.ImageUrl)
                 .FirstOrDefault(),
             Quantity = oi.Quantity
         }).ToList()
     })
     .ToListAsync();

        // ======================
        // 3️⃣ Wishlist Items
        // ======================
        var wishlist = await _db.Wishlists
            .Where(w => w.UserId == userId)
            .Include(w => w.Product)
                //.ThenInclude(p => p.Vendor)
            .Include(w => w.Product)
                .ThenInclude(p => p.ProductImages)
            .Select(w => new DashboardProductDto
            {
                ProductId = w.ProductId,
                ProductName = w.Product.ProductsName,
                Price = w.Product.Price,
                //VendorId = w.Product.VendorId,
                //VendorName = w.Product.Vendor.Name,
                InStock = w.Product.Quantity > 0,
                ThumbnailImageUrl = w.Product.ProductImages
                    .OrderBy(pi => pi.ProductImageId)
                    .Select(pi => pi.ImageUrl)
                    .FirstOrDefault(),
                Quantity = 0 // wishlist have no quantity
            })
            .ToListAsync();

        // ======================
        // 4️⃣ Addresses
        // ======================
        var addresses = await _db.UserAddresses
            .Where(a => a.UserId == userId)
            .Select(a => new DashboardAddressDto
            {
                AddressLine = a.AddressLine1,
                City = a.City,
                PostalCode = a.PostalCode,
                IsDefault = a.IsDefault
            })
            .ToListAsync();

        // ======================
        // Combine all dashboard data
        // ======================
        var dashboard = new CustomerDashboardDto
        {
            Cart = cartItems,
            RecentOrders = recentOrders,
            Wishlist = wishlist,
            Addresses = addresses
        };

        return Ok(dashboard);
    }
}
