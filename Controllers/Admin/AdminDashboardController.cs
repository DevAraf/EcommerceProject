using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorEcommerceProject.Dtos.Admin.Dashboard;

namespace VendorEcommerceProject.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminDashboardController(AppDbContext db)
        {
            _db = db;
        }

        // =================================================
        // DASHBOARD SUMMARY (TOP KPI CARDS)
        // =================================================
        [HttpGet("summary")]
        public async Task<ActionResult<AdminDashboardSummaryDto>> GetSummary()
        {
            var totalOrders = await _db.Orders.CountAsync();

            var totalRevenue = await _db.Payments
                .Where(p => p.Status == "Success")
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            //var totalCommission = await _db.Commissions
            //    .SumAsync(c => (decimal?)c.Amount) ?? 0;

            var pendingProducts = await _db.Products
                .CountAsync(p => p.Status.Name == "Pending");

            //var totalVendorEarning = await _db.VendorEarnings
            //    .SumAsync(v => (decimal?)v.Amount) ?? 0;

            //var totalVendorPaid = await _db.VendorPayments
            //    .SumAsync(vp => (decimal?)vp.Amount) ?? 0;

            var dto = new AdminDashboardSummaryDto
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                //TotalCommission = totalCommission,
                PendingProducts = pendingProducts,
                //PendingVendorPayable = totalVendorEarning - totalVendorPaid
            };

            return Ok(dto);
        }

        // =================================================
        // RECENT ORDERS (LAST 5)
        // =================================================
        [HttpGet("recent-orders")]
        public async Task<ActionResult<List<AdminRecentOrderDto>>> GetRecentOrders()
        {
            var orders = await _db.Orders
                .OrderByDescending(o => o.CreatedAt)
                .Take(5)
                .Select(o => new AdminRecentOrderDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.CreatedAt,
                    CustomerName = o.User.FullName,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status.Name
                })
                .ToListAsync();

            return Ok(orders);
        }



        // =================================================
        // CUSTOMERS
        // =================================================
        //[HttpGet("customers")]
        //public async Task<IActionResult> GetCustomers()
        //{
        //    var customers = await _db.Users
        //        .Where(u => u.CustomerDetails != null)
        //        .Select(u => new
        //        {
        //            u.Id,
        //            u.FullName,
        //            u.Email,
        //            u.IsActive
        //        })
        //        .ToListAsync();

        //    return Ok(customers);
        //}




        // =================================================
        // VENDOR PAYABLE SNAPSHOT
        // =================================================
        //[HttpGet("vendor-payables")]
        //public async Task<ActionResult<List<AdminVendorPayableDto>>> GetVendorPayables()
        //{
        //    var vendors = await _db.Vendors
        //        .Select(v => new
        //        {
        //            v.VendorId,
        //            v.Name,
        //            TotalEarning = v.Earnings.Sum(e => (decimal?)e.Amount) ?? 0,
        //            TotalPaid = v.Payouts.Sum(p => (decimal?)p.Amount) ?? 0
        //        })
        //        .Select(x => new AdminVendorPayableDto
        //        {
        //            VendorId = x.VendorId,
        //            VendorName = x.Name,
        //            PayableAmount = x.TotalEarning - x.TotalPaid
        //        })
        //        .Where(x => x.PayableAmount > 0)
        //        .ToListAsync();

        //    return Ok(vendors);
        //}

        // =================================================
        // COMMISSION SUMMARY
        // =================================================
        //[HttpGet("commission-summary")]
        //public async Task<ActionResult<AdminCommissionSummaryDto>> GetCommissionSummary()
        //{
        //    var totalCommission = await _db.Commissions
        //        .SumAsync(c => (decimal?)c.Amount) ?? 0;

        //    var thisMonthCommission = await _db.Commissions
        //        .Where(c =>
        //            c.CreatedAt.Month == DateTime.UtcNow.Month &&
        //            c.CreatedAt.Year == DateTime.UtcNow.Year)
        //        .SumAsync(c => (decimal?)c.Amount) ?? 0;

        //    var dto = new AdminCommissionSummaryDto
        //    {
        //        TotalCommission = totalCommission,
        //        ThisMonthCommission = thisMonthCommission
        //    };

        //    return Ok(dto);
        //}
    }
}
