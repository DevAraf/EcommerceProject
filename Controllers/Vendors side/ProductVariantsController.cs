using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendorEcommerceProject.Dtos.Vendor.ProductVariants;
using VendorEcommerceProject.Dtos.Vendor.ProductVariants.VendorProductVariantBulk;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models.ProductsTables;

[ApiController]
[Route("api/vendor/product-variants")]
[Authorize(Roles = "Admin")]
public class ProductVariantsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductVariantsController(AppDbContext db)
    {
        _db = db;
    }

    // ----------------------------------------
    // GET: Variants of a product (vendor only)
    // ----------------------------------------
    //[HttpGet("{productId:long}")]
    //public async Task<IActionResult> GetByProduct(long productId)
    //{
    //    long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    //    var product = await _db.Products
    //        .Include(p => p.Vendor)
    //        .FirstOrDefaultAsync(p => p.ProductId == productId && p.Vendor.UserId == userId);

    //    if (product == null)
    //        return NotFound("Product not found".SendResponse());

    //    var variants = await _db.ProductVariants
    //        .Where(v => v.ProductId == productId)
    //        .Select(v => new VendorProductVariantListDto
    //        {
    //            ProductVariantId = v.ProductVariantId,
    //            AttributeName = v.Attribute.Name,
    //            Value = v.Value,
    //            AdditionalPrice = v.AdditionalPrice,
    //            Quantity = v.Quantity
    //        })
    //        .ToListAsync();

    //    return Ok(variants);
    //}
    [HttpGet("{productId:long}")]
    public async Task<IActionResult> GetByProduct(long productId)
    {
        var product = await _db.Products
            .FirstOrDefaultAsync(p => p.ProductId == productId);

        if (product == null)
            return NotFound("Product not found".SendResponse());

        var variants = await _db.ProductVariants
            .Where(v => v.ProductId == productId)
            .Select(v => new VendorProductVariantListDto
            {
                ProductVariantId = v.ProductVariantId,
                AttributeName = v.Attribute.Name,
                Value = v.Value,
                Price = v.Price,
                Quantity = v.Quantity
            })
            .ToListAsync();

        return Ok(variants);
    }


    // ----------------------------------------
    // POST: Variants of a product array type uopload
    // ----------------------------------------

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate(VendorProductVariantBulkCreateDto dto)
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // 1️ Check product ownership
        var product = await _db.Products
            //.Include(p => p.Vendor)
            .FirstOrDefaultAsync(p =>
                p.ProductId == dto.ProductId);
                //p.Vendor.UserId == userId);

        if (product == null)
            return BadRequest("Invalid product".SendResponse());

        if (dto.Variants == null || !dto.Variants.Any())
            return BadRequest("No variants provided".SendResponse());

        // 2️ Prevent duplicate values in request (XL, XL)
        var duplicateValues = dto.Variants
            .GroupBy(v => new { v.ProductAttributeId, v.Value })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key.Value)
            .ToList();

        if (duplicateValues.Any())
            return BadRequest($"Duplicate variant values found: {string.Join(", ", duplicateValues)}".SendResponse());

        // 3️ Prevent duplicate variants already in DB
        var existingVariants = await _db.ProductVariants
            .Where(v => v.ProductId == dto.ProductId)
            .Select(v => new { v.ProductAttributeId, v.Value })
            .ToListAsync();

        var conflicts = dto.Variants.Any(v =>
            existingVariants.Any(e =>
                e.ProductAttributeId == v.ProductAttributeId &&
                e.Value == v.Value));

        if (conflicts)
            return BadRequest("One or more variants already exist".SendResponse());

        // 4️ Create variants
        var newVariants = dto.Variants.Select(v => new ProductVariant
        {
            ProductId = dto.ProductId,
            ProductAttributeId = v.ProductAttributeId,
            Value = v.Value,
            Price = v.AdditionalPrice,
            Quantity = v.Quantity,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        _db.ProductVariants.AddRange(newVariants);
        await _db.SaveChangesAsync();

        return Ok("Variants added successfully".SendResponse());
    }

    // ----------------------------------------
    // POST: Create variant
    // ----------------------------------------
    [HttpPost]
    public async Task<IActionResult> Create(VendorProductVariantCreateDto dto)
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var product = await _db.Products
            //.Include(p => p.Vendor)
            .FirstOrDefaultAsync(p =>
                p.ProductId == dto.ProductId);
                //p.Vendor.UserId == userId);

        if (product == null)
            return BadRequest("Invalid product".SendResponse());

        var variant = new ProductVariant
        {
            ProductId = dto.ProductId,
            ProductAttributeId = dto.ProductAttributeId,
            Value = dto.Value,
            Price = dto.Price,
            Quantity = dto.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        _db.ProductVariants.Add(variant);
        await _db.SaveChangesAsync();

        return Ok("Variant added successfully".SendResponse());
    }

    // ----------------------------------------
    // PUT: Update variant
    // ----------------------------------------
    [HttpPut]
    public async Task<IActionResult> Update(VendorProductVariantUpdateDto dto)
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var variant = await _db.ProductVariants
            .Include(v => v.Product)
            //.ThenInclude(p => p.Vendor)
            .FirstOrDefaultAsync(v =>
                v.ProductVariantId == dto.ProductVariantId);
                //v.Product.Vendor.UserId == userId);

        if (variant == null)
            return NotFound("Variant not found".SendResponse());

        variant.Value = dto.Value;
        variant.Price = dto.Price;
        variant.Quantity = dto.Quantity;
        variant.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok("Variant updated".SendResponse());
    }

    // ----------------------------------------
    // DELETE: Remove variant
    // ----------------------------------------
    //[HttpDelete("{id:long}")]
    //public async Task<IActionResult> Delete(long id)
    //{
    //    long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    //    var variant = await _db.ProductVariants
    //        .Include(v => v.Product)
    //        .ThenInclude(p => p.Vendor)
    //        .FirstOrDefaultAsync(v =>
    //            v.ProductVariantId == id &&
    //            v.Product.Vendor.UserId == userId);

    //    if (variant == null)
    //        return NotFound();

    //    _db.ProductVariants.Remove(variant);
    //    await _db.SaveChangesAsync();

    //    return Ok("Variant deleted".SendResponse());
    //}
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var variant = await _db.ProductVariants
            .Include(v => v.Product)
            .FirstOrDefaultAsync(v => v.ProductVariantId == id);

        if (variant == null)
            return NotFound();

        _db.ProductVariants.Remove(variant);

        await _db.SaveChangesAsync();

        return Ok("Variant deleted".SendResponse());
    }
}
