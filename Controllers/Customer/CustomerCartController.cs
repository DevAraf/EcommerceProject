using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendorEcommerceProject.Dtos.Customer.Cart;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models.OrdersAndCartTable;

[ApiController]
[Route("api/customer/cart")]
[Authorize(Roles = "Customer")]
public class CustomerCartController : ControllerBase
{
    private readonly AppDbContext _db;

    public CustomerCartController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var cart = await _db.Carts
            .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
            .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                    .ThenInclude(p => p.ProductImages)
            .Include(c => c.Items)
                .ThenInclude(ci => ci.CartItemVariants)
                    .ThenInclude(civ => civ.ProductVariant)
                        .ThenInclude(pv => pv.Attribute)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            return Ok(new
            {
                CartId = 0,
                Items = new List<object>(),
                SubTotal = 0
            });
        }

        return Ok(new
        {
            CartId = cart.CartId,

            Items = cart.Items.Select(ci =>
            {
                var variantPrice = ci.CartItemVariants
                    .Select(v => v.ProductVariant.Price)
                    .FirstOrDefault();

                return new
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.ProductsName,
                    Quantity = ci.Quantity,

                    UnitPrice = variantPrice,
                    TotalPrice = ci.Quantity * variantPrice,

                    Variants = ci.CartItemVariants.Select(v => new
                    {
                        VariantId = v.ProductVariant.ProductVariantId,
                        Attribute = v.ProductVariant.Attribute.Name,
                        Value = v.ProductVariant.Value,
                        Price = v.ProductVariant.Price
                    }).ToList(),

                    FirstImageUrl = ci.Product.ProductImages
                        .OrderBy(pi => pi.ProductImageId)
                        .Select(pi => pi.ImageUrl)
                        .FirstOrDefault()
                };
            }).ToList(),

            SubTotal = cart.Items.Sum(ci =>
                ci.Quantity *
                ci.CartItemVariants
                    .Select(v => v.ProductVariant.Price)
                    .FirstOrDefault())
        });
    }

    // GET cart
    //[HttpGet]
    //public async Task<IActionResult> GetCart()
    //{
    //    long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    //    var cart = await _db.Carts
    //        .Include(c => c.Items)
    //            .ThenInclude(ci => ci.Product)
    //                //.ThenInclude(p => p.Vendor)
    //        .Include(c => c.Items)
    //            .ThenInclude(ci => ci.Product)
    //                .ThenInclude(p => p.ProductImages)
    //        .Include(c => c.Items)
    //            .ThenInclude(ci => ci.CartItemVariants)
    //                .ThenInclude(civ => civ.ProductVariant)
    //                    .ThenInclude(pv => pv.Attribute)
    //        .FirstOrDefaultAsync(c => c.UserId == userId);

    //    if (cart == null)
    //        return Ok(new { CartId = 0, /*Vendors = new List<object>()*/ });


    //    return Ok(new
    //    {
    //        CartId = cart.CartId,
    //        Items = cart.Items.Select(ci => new
    //        {
    //            CartItemId = ci.CartItemId,
    //            ProductId = ci.ProductId,
    //            ProductName = ci.Product.ProductsName,
    //            Quantity = ci.Quantity,
    //            UnitPrice = ci.Product.Price,
    //            TotalPrice = ci.Quantity * ci.Product.Price,
    //            Variants = ci.CartItemVariants.Select(v => new
    //            {
    //                Attribute = v.ProductVariant.Attribute.Name,
    //                Value = v.ProductVariant.Value
    //            }),
    //            FirstImageUrl = ci.Product.ProductImages
    //                .OrderBy(pi => pi.ProductImageId)
    //                .Select(pi => pi.ImageUrl)
    //                .FirstOrDefault()
    //        }).ToList(),
    //        SubTotal = cart.Items.Sum(ci => ci.Quantity * ci.Product.Price)
    //    });
    //}

    // POST add product to cart
    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] AddCartItemRequestDto request)
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Get or create cart
        var cart = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId, CreatedAt = DateTime.UtcNow };
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync();
        }

        // Pull all cart items of this product to memory
        var cartItems = await _db.CartItems
            .Include(ci => ci.CartItemVariants)
            .Where(ci => ci.CartId == cart.CartId && ci.ProductId == request.ProductId)
            .ToListAsync();  // <-- move query to memory

        // Prepare selected variant IDs
        var selectedVariantIds = request.Variants.Select(v => v.VariantId).OrderBy(id => id).ToList();

        // Check if same variant combination exists in memory
        var cartItem = cartItems.FirstOrDefault(ci =>
            ci.CartItemVariants.Select(civ => civ.ProductVariantId).OrderBy(id => id)
                .SequenceEqual(selectedVariantIds)
        );

        if (cartItem != null)
        {
            // Same combination exists → increase quantity
            cartItem.Quantity += request.Quantity;
        }
        else
        {
            // New combination → create new CartItem
            cartItem = new CartItem
            {
                CartId = cart.CartId,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };
            _db.CartItems.Add(cartItem);
            await _db.SaveChangesAsync(); // Save to get CartItemId

            // Add CartItemVariants
            foreach (var sel in request.Variants)
            {
                var variant = await _db.ProductVariants
                    .FirstOrDefaultAsync(v =>
                        v.ProductVariantId == sel.VariantId &&
                        v.Quantity >= request.Quantity
                    );

                if (variant == null)
                    return BadRequest($"Variant {sel.VariantId} (Attribute {sel.AttributeId}) out of stock".SendResponse());

                _db.CartItemVariants.Add(new CartItemVariant
                {
                    CartItemId = cartItem.CartItemId,
                    ProductVariantId = variant.ProductVariantId
                });
            }
        }

        await _db.SaveChangesAsync();
        return Ok("Product(s) added to cart successfully".SendResponse());
    }



    // PUT update cart item quantity
    [HttpPut("{cartItemId}")]
    public async Task<IActionResult> UpdateCartItem(long cartItemId, [FromBody] UpdateCartItemRequestDto request)
    {
        var cartItem = await _db.CartItems.FindAsync(cartItemId);
        if (cartItem == null) return NotFound();

        cartItem.Quantity = request.Quantity;
        await _db.SaveChangesAsync();
        return Ok("Cart item updated".SendResponse());
    }

    // DELETE remove product from cart
    [HttpDelete("{cartItemId}")]
    public async Task<IActionResult> RemoveCartItem(long cartItemId)
    {
        var cartItem = await _db.CartItems
            .Include(ci => ci.CartItemVariants)
            .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

        if (cartItem == null) return NotFound();

        _db.CartItemVariants.RemoveRange(cartItem.CartItemVariants);
        _db.CartItems.Remove(cartItem);
        await _db.SaveChangesAsync();
        return Ok("Product removed from cart".SendResponse());
    }
}

// Request DTOs
//public class AddCartItemRequestDto
//{
//    public long ProductId { get; set; }
//    public int Quantity { get; set; } = 1;
//    public List<VariantDto> Variants { get; set; } = new List<VariantDto>();
//}

//public class VariantDto
//{
//    public long VariantId { get; set; }      // ProductVariantId
//    public long AttributeId { get; set; }    // optional, for info
//}

//public class UpdateCartItemRequestDto
//{
//    public int Quantity { get; set; }
//}
