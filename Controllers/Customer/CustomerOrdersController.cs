using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendorEcommerceProject.Dtos.Customer.Orders;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models.OrdersAndCartTable;
using VendorEcommerceProject.Models.PaymentsTable;
using VendorEcommerceProject.Models.UserDetailsTable;

[ApiController]
[Route("api/customer/orders")]
[Authorize(Roles = "Customer")]
public class CustomerOrdersController : ControllerBase
{
    private readonly AppDbContext _db;

    public CustomerOrdersController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var orders = await _db.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.ProductImages)
            .Include(o => o.Status)
            
            .Include(o => o.PaymentMethod)
            .Where(o => o.UserId == userId)
            .ToListAsync();

        var result = orders.Select(o => new
        {
            OrderId = o.OrderId,
            CreatedAt = o.CreatedAt,
            Status = o.Status.Name,
            Payment =o.PaymentMethod.Name,
            Items = o.OrderItems.Select(oi => new
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product.ProductsName,
                Quantity = oi.Quantity,
                Price = oi.Price,
                FirstImageUrl = oi.Product.ProductImages.OrderBy(pi => pi.ProductImageId).Select(pi => pi.ImageUrl).FirstOrDefault()
            }).ToList(),
            TotalAmount = o.OrderItems.Sum(oi => oi.Quantity * oi.Price)
        });

        return Ok(result);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderDetails(long orderId)
    {
        long userId = long.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var order = await _db.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.ProductImages)
            .Include(o => o.Status)
            .Include(o => o.PaymentMethod)
            .Include(o => o.ShippingAddress)
            .Include(o => o.Payments)
            .FirstOrDefaultAsync(o =>
                o.OrderId == orderId &&
                o.UserId == userId);

        if (order == null)
            return NotFound("Order not found".SendResponse());

        var result = new
        {
            OrderId = order.OrderId,
            CreatedAt = order.CreatedAt,

            Status = order.Status.Name,

            PaymentMethod = order.PaymentMethod.Name,

            Payment = order.Payments.Select(p => new
            {
                PaymentId = p.PaymentId,
                Amount = p.Amount,
                Status = p.Status,
                CreatedAt = p.CreatedAt
            }).FirstOrDefault(),

            ShippingAddress = order.ShippingAddress == null
                ? null
                : new
                {
                    AddressId = order.ShippingAddress.UserAddressId,
                    AddressLine1 = order.ShippingAddress.AddressLine1,
                    AddressLine2 = order.ShippingAddress.AddressLine2,
                    City = order.ShippingAddress.City,
                    PostalCode = order.ShippingAddress.PostalCode,
                    Country = order.ShippingAddress.Country
                },

            Items = order.OrderItems.Select(oi => new
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product.ProductsName,
                Quantity = oi.Quantity,
                Price = oi.Price,

                FirstImageUrl = oi.Product.ProductImages
                    .OrderBy(pi => pi.ProductImageId)
                    .Select(pi => pi.ImageUrl)
                    .FirstOrDefault()
            }).ToList(),

            TotalAmount = order.OrderItems
                .Sum(oi => oi.Quantity * oi.Price)
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDto dto)
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // 1. Get cart
        var cart = await _db.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .Include(c => c.Items)
                .ThenInclude(i => i.CartItemVariants)
                    .ThenInclude(civ => civ.ProductVariant)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.Items.Any())
            return BadRequest("Cart is empty".SendResponse());

        //// 2. Save new shipping address
        //if (dto.SetAsDefault)
        //{
        //    var oldDefaults = await _db.UserAddresses
        //        .Where(a => a.UserId == userId && a.IsDefault)
        //        .ToListAsync();

        //    oldDefaults.ForEach(a => a.IsDefault = false);
        //}

        //var address = new UserAddress
        //{
        //    UserId = userId,
        //    AddressLine1 = dto.AddressLine1,
        //    AddressLine2 = dto.AddressLine2,
        //    City = dto.City,
        //    PostalCode = dto.PostalCode,
        //    Country = dto.Country,
        //    IsDefault = dto.SetAsDefault
        //};

        //_db.UserAddresses.Add(address);
        //await _db.SaveChangesAsync();

        // 2. Get Shipping Address
        //UserAddress? address;

        //if (dto.UserAddressId.HasValue)
        //{
        //    // Customer selected an existing address
        //    address = await _db.UserAddresses
        //        .FirstOrDefaultAsync(a =>
        //            a.UserAddressId == dto.UserAddressId.Value &&
        //            a.UserId == userId);

        //    if (address == null)
        //        return BadRequest("Invalid shipping address".SendResponse());
        //}
        //else
        //{
        //    // No address selected → use customer's default address
        //    address = await _db.UserAddresses
        //        .FirstOrDefaultAsync(a =>
        //            a.UserId == userId &&
        //            a.IsDefault);

        //    if (address == null)
        //        return BadRequest("No default shipping address found".SendResponse());
        //}

        UserAddress? address;

        if (dto.UserAddressId.HasValue)
        {
            address = await _db.UserAddresses
                .FirstOrDefaultAsync(a =>
                    a.UserAddressId == dto.UserAddressId.Value &&
                    a.UserId == userId);

            if (address == null)
                return BadRequest("Invalid shipping address".SendResponse());
        }
        else
        {
            address = await _db.UserAddresses
                .FirstOrDefaultAsync(a =>
                    a.UserId == userId &&
                    a.IsDefault);

            if (address == null)
                return BadRequest("No default shipping address found".SendResponse());
        }


        // 3. Validate Payment Method
        var paymentMethod = await _db.PaymentMethods
            .FirstOrDefaultAsync(p =>
                p.PaymentMethodId == dto.PaymentMethodId &&
                p.IsActive);

        if (paymentMethod == null)
            return BadRequest("Invalid or inactive payment method".SendResponse());

        // 3. Get Pending Status
        var pendingStatusId = await _db.OrderStatuses
            .Where(s => s.Name == "Pending")
            .Select(s => s.OrderStatusId)
            .FirstOrDefaultAsync();

        if (pendingStatusId == 0)
            return BadRequest("Pending order status not found".SendResponse());

        // 4. Create Order
        var order = new Orders
        {
            //UserId = userId,
            //ShippingAddressId = address.UserAddressId,
            //OrderStatusId = pendingStatusId,
            //PaymentMethodId = paymentMethod.PaymentMethodId,
            //CreatedAt = DateTime.UtcNow

            UserId = userId,
            ShippingAddressId = address.UserAddressId,
            OrderStatusId = pendingStatusId,
            PaymentMethodId = paymentMethod.PaymentMethodId,
            CreatedAt = DateTime.UtcNow
        };

      

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        // 5. Add Order Items
        foreach (var item in cart.Items)
        {
            var variantPrice = item.CartItemVariants
                .Select(v => v.ProductVariant.Price)
                .FirstOrDefault();

            _db.OrderItems.Add(new OrderItem
            {
                OrderId = order.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = variantPrice
            });
        }

        await _db.SaveChangesAsync();

        

        // 6. Create Payment Record
        var totalAmount = cart.Items.Sum(item =>
        {
            var variantPrice = item.CartItemVariants
                .Select(v => v.ProductVariant.Price)
                .FirstOrDefault();

            return variantPrice * item.Quantity;
        });

        var payment = new Payment
        {
            OrderId = order.OrderId,
            PaymentMethodId = paymentMethod.PaymentMethodId,
            Amount = totalAmount,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        // 7. Clear Cart
        _db.CartItems.RemoveRange(cart.Items);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            orderId = order.OrderId,
            message = "Order placed successfully",
            shippingAddress = $"{address.AddressLine1}, {address.City}, {address.Country}"
        });

        //// 6. Clear Cart
        //_db.CartItems.RemoveRange(cart.Items);
        //await _db.SaveChangesAsync();



        //return Ok(new
        //{
        //    orderId = order.OrderId,
        //    message = "Order placed successfully",
        //    shippingAddress = $"{address.AddressLine1}, {address.City}, {address.Country}"
        //});
    }


}
