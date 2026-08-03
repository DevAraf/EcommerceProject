using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendorEcommerceProject.Dtos.Vendor.ProductTags;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models.ProductsTables;

[ApiController]
[Route("api/vendor/product-tags")]
[Authorize(Roles = "Admin")]
public class ProductTagsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductTagsController(AppDbContext db)
    {
        _db = db;
    }

    // ---------------------------------------
    // GET: Tags of a product (vendor only)
    // ---------------------------------------
    [HttpGet("{productId:long}")]
    public async Task<IActionResult> GetProductTags(long productId)
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var product = await _db.Products
            //.Include(p => p.Vendor)
            .Include(p => p.ProductTags)
            .FirstOrDefaultAsync(p =>
                p.ProductId == productId);
                //p.Vendor.UserId == userId);

        if (product == null)
            return NotFound("Product not found".SendResponse());

        var tags = product.ProductTags
            .Select(t => new VendorProductTagListDto
            {
                ProductTagId = t.ProductTagId,
                Name = t.Name
            })
            .ToList();

        return Ok(tags);
    }

    // ---------------------------------------
    // POST: Assign / Replace tags
    // ---------------------------------------
    [HttpPost("assign")]
    public async Task<IActionResult> AssignTags(VendorProductTagAssignDto dto)
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var product = await _db.Products
            //.Include(p => p.Vendor)
            .Include(p => p.ProductTags)
            .FirstOrDefaultAsync(p =>
                p.ProductId == dto.ProductId);
                //p.Vendor.UserId == userId);

        if (product == null)
            return BadRequest("Invalid product".SendResponse());

        var tags = await _db.ProductTags
            .Where(t => dto.ProductTagIds.Contains(t.ProductTagId))
            .ToListAsync();

        product.ProductTags.Clear();
        foreach (var tag in tags)
            product.ProductTags.Add(tag);

        product.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok("Tags assigned successfully".SendResponse());
    }
}
