using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

using VendorEcommerceProject.Helpers;

namespace VendorEcommerceProject.Controllers.Vendorside
{
    [ApiController]
    [Route("api/vendor/dashboard")]
    [Authorize(Roles = "Vendor")]
    public class VendorDashboardController : ControllerBase
    {
        private readonly AppDbContext _db;

        public VendorDashboardController(AppDbContext db)
        {
            _db = db;
        }

        // =================================================
        // 1️⃣ DASHBOARD SUMMARY
        // =================================================
        //[HttpGet("summary")]
        //public async Task<ActionResult<VendorDashboardSummaryDto>> GetSummary()
        //{
        //    long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        //    long vendorId = await _db.Vendors
        //        .Where(v => v.UserId == userId)
        //        .Select(v => v.VendorId)
        //        .FirstOrDefaultAsync();

        //    if (vendorId == 0)
        //        return BadRequest("Vendor account not found".SendResponse());

        //    int totalProducts = await _db.Products
        //        .CountAsync(p => p.VendorId == vendorId && p.DeletedAt == null);

        //    int approvedProducts = await _db.Products
        //        .CountAsync(p => p.VendorId == vendorId && p.Status.Name == "Approved");

        //    int pendingProducts = await _db.Products
        //        .CountAsync(p => p.VendorId == vendorId && p.Status.Name == "Pending");

        //    decimal totalEarnings = await _db.VendorEarnings
        //        .Where(e => e.VendorId == vendorId)
        //        .SumAsync(e => (decimal?)e.Amount) ?? 0;

        //    decimal totalPaid = await _db.VendorPayments
        //        .Where(p => p.VendorId == vendorId)
        //        .SumAsync(p => (decimal?)p.Amount) ?? 0;

        //    return Ok(new VendorDashboardSummaryDto
        //    {
        //        TotalProducts = totalProducts,
        //        ApprovedProducts = approvedProducts,
        //        PendingProducts = pendingProducts,
        //        TotalEarnings = totalEarnings,
        //        PayableAmount = totalEarnings - totalPaid
        //    });
        //}

        // =================================================
        // 2️⃣ RECENT ORDERS
        // =================================================
        //[HttpGet("recent-orders")]
        //public async Task<ActionResult<List<VendorRecentOrderDto>>> GetRecentOrders()
        //{
        //    long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        //    //long vendorId = await _db.Vendors
        //    //    .Where(v => v.UserId == userId)
        //    //    .Select(v => v.VendorId)
        //    //    .FirstOrDefaultAsync();

        //    //if (vendorId == 0)
        //    //    return BadRequest("Vendor account not found".SendResponse());

        //    var orders = await _db.OrderItems
        //        //.Where(oi => oi.Product.VendorId == vendorId)
        //        .OrderByDescending(oi => oi.CreatedAt)
        //        .Take(5)
        //        .Select(oi => new VendorRecentOrderDto
        //        {
        //            OrderId = oi.OrderId,
        //            ProductName = oi.Product.ProductsName,
        //            Quantity = oi.Quantity,
        //            Amount = oi.Price * oi.Quantity,
        //            OrderDate = oi.CreatedAt
        //        })
        //        .ToListAsync();

        //    return Ok(orders);
        //}

        // =================================================
        // 3️⃣ PRODUCT STATUS SUMMARY
        // =================================================
        //[HttpGet("product-status")]
        //public async Task<ActionResult<VendorProductStatusSummaryDto>> GetProductStatus()
        //{
        //    long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        //    long vendorId = await _db.Vendors
        //        .Where(v => v.UserId == userId)
        //        .Select(v => v.VendorId)
        //        .FirstOrDefaultAsync();

        //    //if (vendorId == 0)
        //    //    return BadRequest("Vendor account not found".SendResponse());

        //    //int approved = await _db.Products
        //    //    .CountAsync(p => p.VendorId == vendorId && p.Status.Name == "Approved");

        //    //int pending = await _db.Products
        //    //    .CountAsync(p => p.VendorId == vendorId && p.Status.Name == "Pending");

        //    //int rejected = await _db.Products
        //    //    .CountAsync(p => p.VendorId == vendorId && p.Status.Name == "Rejected");

        //    //return Ok(new VendorProductStatusSummaryDto
        //    //{
        //    //    Approved = approved,
        //    //    Pending = pending,
        //    //    Rejected = rejected
        //    //});
        //}

        //[HttpGet("product-status")]
        //public async Task<ActionResult<VendorProductStatusSummaryDto>> GetProductStatus()
        //{
        //    int approved = await _db.Products
        //        .CountAsync(p => p.Status.Name == "Approved");

        //    int pending = await _db.Products
        //        .CountAsync(p => p.Status.Name == "Pending");

        //    int rejected = await _db.Products
        //        .CountAsync(p => p.Status.Name == "Rejected");

        //    return Ok(new VendorProductStatusSummaryDto
        //    {
        //        Approved = approved,
        //        Pending = pending,
        //        Rejected = rejected
        //    });
        //}
    }
}
