using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Controllers.Admin.Order
{
    [Route("api/admin/orders")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminOrdersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminOrdersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.Status)
                .Include(o => o.PaymentMethod)
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var result = orders.Select(o => new
            {
                OrderId = o.OrderId,
                CustomerName = o.User.UserName,
                CreatedAt = o.CreatedAt,
                Status = o.Status.Name,
                PaymentMethod = o.PaymentMethod.Name,
                ShippingAddress = o.ShippingAddress == null
                    ? null
                    : $"{o.ShippingAddress.AddressLine1}, {o.ShippingAddress.City}, {o.ShippingAddress.Country}",
                Items = o.OrderItems.Select(oi => new
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.ProductsName,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList(),
                TotalAmount = o.OrderItems.Sum(oi => oi.Quantity * oi.Price)
            });

            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetOrderDetails(long id)
        {
            var order = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.Status)
                .Include(o => o.PaymentMethod)
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound("Order not found");

            var result = new
            {
                OrderId = order.OrderId,
                CustomerName = order.User.UserName,
                CreatedAt = order.CreatedAt,
                Status = order.Status.Name,
                PaymentMethod = order.PaymentMethod.Name,

                ShippingAddress = order.ShippingAddress == null
                    ? null
                    : $"{order.ShippingAddress.AddressLine1}, {order.ShippingAddress.City}, {order.ShippingAddress.Country}",

                Items = order.OrderItems.Select(oi => new
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.ProductsName,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList(),

                TotalAmount = order.OrderItems
                    .Sum(oi => oi.Quantity * oi.Price)
            };

            return Ok(result);
        }

        [HttpPut("{id:long}/status")]
        public async Task<IActionResult> UpdateOrderStatus(
    long id,
    [FromBody] long orderStatusId)
        {
            var order = await _db.Orders
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound("Order not found");

            var statusExists = await _db.OrderStatuses
                .AnyAsync(s => s.OrderStatusId == orderStatusId);

            if (!statusExists)
                return BadRequest("Invalid order status");

            order.OrderStatusId = orderStatusId;
            order.ModifiedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Order status updated successfully",
                orderId = order.OrderId,
                orderStatusId = order.OrderStatusId
            });
        }
    }
}