using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models;
using VendorEcommerceProject.Models.UserDetailsTable;

[ApiController]
[Route("api/customer/wishlist")]
[Authorize(Roles = "Customer")]
public class CustomerWishlistController : ControllerBase
{
    private readonly AppDbContext _db;

    public CustomerWishlistController(AppDbContext db)
    {
        _db = db;
    }

    // GET wishlist
    [HttpGet]
    public async Task<IActionResult> GetWishlist()
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var wishlist = await _db.Wishlists
            .Include(w => w.Product)
                .ThenInclude(p => p.ProductImages)
            .Where(w => w.UserId == userId)
            .ToListAsync();

        var result = wishlist.Select(w => new
        {
            ProductId = w.ProductId,
            ProductName = w.Product.ProductsName,
            Price = w.Product.Price,
            FirstImageUrl = w.Product.ProductImages.OrderBy(pi => pi.ProductImageId).Select(pi => pi.ImageUrl).FirstOrDefault()
        });

        return Ok(result);
    }

    // POST add to wishlist
    [HttpPost]
    public async Task<IActionResult> AddToWishlist([FromBody] WishlistRequest request)
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var exists = await _db.Wishlists.AnyAsync(w => w.UserId == userId && w.ProductId == request.ProductId);
        if (exists) return BadRequest("Already in wishlist".SendResponse());

        _db.Wishlists.Add(new Wishlist { UserId = userId, ProductId = request.ProductId });
        await _db.SaveChangesAsync();
        return Ok("Product added to wishlist".SendResponse());
    }

    // DELETE remove from wishlist
    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveFromWishlist(long productId)
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var wishlist = await _db.Wishlists.FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
        if (wishlist == null) return NotFound();

        _db.Wishlists.Remove(wishlist);
        await _db.SaveChangesAsync();
        return Ok("Product removed from wishlist".SendResponse());
    }
}

public class WishlistRequest
{
    public long ProductId { get; set; }
}
