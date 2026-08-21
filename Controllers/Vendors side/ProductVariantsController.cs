using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


    // ============================================================
    // GET: Get all product variants
    // ============================================================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var variants = await _db.ProductVariants
            .OrderBy(v => v.ProductId)
            .ThenBy(v => v.ParentVariantId)
            .ThenBy(v => v.ProductVariantId)
            .Select(v => new VendorProductVariantListDto
            {
                ProductVariantId = v.ProductVariantId,

                ParentVariantId = v.ParentVariantId,

                AttributeName = v.Attribute.Name,

                Value = v.Value,

                Price = v.Price,

                Quantity = v.Quantity
            })
            .ToListAsync();

        return Ok(variants);
    }

    // ============================================================
    // GET: Get all variants of a product
    // ============================================================
    [HttpGet("{productId:long}")]
    public async Task<IActionResult> GetByProduct(long productId)
    {
        var productExists = await _db.Products
            .AnyAsync(p => p.ProductId == productId);

        if (!productExists)
            return NotFound("Product not found".SendResponse());

        var variants = await _db.ProductVariants
            .Where(v => v.ProductId == productId)
            .OrderBy(v => v.ParentVariantId)
            .ThenBy(v => v.ProductVariantId)
            .Select(v => new VendorProductVariantListDto
            {
                ProductVariantId = v.ProductVariantId,

                ParentVariantId = v.ParentVariantId,

                AttributeName = v.Attribute.Name,

                Value = v.Value,

                Price = v.Price,

                Quantity = v.Quantity
            })
            .ToListAsync();

        return Ok(variants);
    }


    // ============================================================
    // POST: Create multiple variants
    // ============================================================
    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate(
        VendorProductVariantBulkCreateDto dto)
    {
        var productExists = await _db.Products
            .AnyAsync(p => p.ProductId == dto.ProductId);

        if (!productExists)
            return BadRequest("Invalid product".SendResponse());

        if (dto.Variants == null || !dto.Variants.Any())
            return BadRequest("No variants provided".SendResponse());


        // --------------------------------------------------------
        // Validate all parent variants
        // --------------------------------------------------------

        var parentIds = dto.Variants
            .Where(v => v.ParentVariantId.HasValue)
            .Select(v => v.ParentVariantId!.Value)
            .Distinct()
            .ToList();

        if (parentIds.Any())
        {
            var validParents = await _db.ProductVariants
                .Where(v =>
                    parentIds.Contains(v.ProductVariantId) &&
                    v.ProductId == dto.ProductId)
                .Select(v => new
                {
                    v.ProductVariantId,
                    v.ParentVariantId
                })
                .ToListAsync();

            if (validParents.Count != parentIds.Count)
            {
                return BadRequest(
                    "One or more parent variants are invalid"
                    .SendResponse());
            }

            // Parent itself must be a root variant
            if (validParents.Any(v => v.ParentVariantId.HasValue))
            {
                return BadRequest(
                    "A child variant cannot be used as a parent"
                    .SendResponse());
            }
        }


        // --------------------------------------------------------
        // Prevent duplicate variants inside request
        //
        // Same Product + Parent + Attribute + Value
        // --------------------------------------------------------

        var duplicateVariants = dto.Variants
            .GroupBy(v => new
            {
                v.ParentVariantId,
                v.ProductAttributeId,
                v.Value
            })
            .Where(g => g.Count() > 1)
            .ToList();

        if (duplicateVariants.Any())
        {
            return BadRequest(
                "Duplicate variants found in request"
                .SendResponse());
        }


        // --------------------------------------------------------
        // Check existing variants in database
        // --------------------------------------------------------

        var existingVariants = await _db.ProductVariants
            .Where(v => v.ProductId == dto.ProductId)
            .Select(v => new
            {
                v.ParentVariantId,
                v.ProductAttributeId,
                v.Value
            })
            .ToListAsync();


        var hasConflict = dto.Variants.Any(requestVariant =>
            existingVariants.Any(existingVariant =>
                existingVariant.ParentVariantId ==
                    requestVariant.ParentVariantId &&

                existingVariant.ProductAttributeId ==
                    requestVariant.ProductAttributeId &&

                existingVariant.Value ==
                    requestVariant.Value
            )
        );


        if (hasConflict)
        {
            return BadRequest(
                "One or more variants already exist"
                .SendResponse());
        }


        // --------------------------------------------------------
        // Create variants
        // --------------------------------------------------------

        var newVariants = dto.Variants
            .Select(v => new ProductVariant
            {
                ProductId = dto.ProductId,

                ProductAttributeId =
                    v.ProductAttributeId,

                Value = v.Value,

                ParentVariantId =
                    v.ParentVariantId,

                Price = v.AdditionalPrice,

                Quantity = v.Quantity,

                CreatedAt = DateTime.UtcNow
            })
            .ToList();


        _db.ProductVariants.AddRange(newVariants);

        await _db.SaveChangesAsync();

        return Ok(
            "Variants added successfully"
            .SendResponse());
    }


    // ============================================================
    // POST: Create single variant
    // ============================================================
    [HttpPost]
    public async Task<IActionResult> Create(
        VendorProductVariantCreateDto dto)
    {
        // --------------------------------------------------------
        // Check product
        // --------------------------------------------------------

        var productExists = await _db.Products
            .AnyAsync(p => p.ProductId == dto.ProductId);

        if (!productExists)
            return BadRequest(
                "Invalid product".SendResponse());


        // --------------------------------------------------------
        // Parent validation
        // --------------------------------------------------------

        if (dto.ParentVariantId.HasValue)
        {
            var parentVariant = await _db.ProductVariants
                .FirstOrDefaultAsync(v =>
                    v.ProductVariantId ==
                        dto.ParentVariantId.Value &&

                    v.ProductId ==
                        dto.ProductId);

            if (parentVariant == null)
            {
                return BadRequest(
                    "Invalid parent variant"
                    .SendResponse());
            }


            // A child cannot become a parent
            if (parentVariant.ParentVariantId.HasValue)
            {
                return BadRequest(
                    "A child variant cannot be used as a parent"
                    .SendResponse());
            }
        }


        // --------------------------------------------------------
        // Duplicate check
        // --------------------------------------------------------

        var exists = await _db.ProductVariants
            .AnyAsync(v =>
                v.ProductId == dto.ProductId &&

                v.ParentVariantId ==
                    dto.ParentVariantId &&

                v.ProductAttributeId ==
                    dto.ProductAttributeId &&

                v.Value == dto.Value);


        if (exists)
        {
            return BadRequest(
                "Variant already exists"
                .SendResponse());
        }


        // --------------------------------------------------------
        // Create
        // --------------------------------------------------------

        var variant = new ProductVariant
        {
            ProductId = dto.ProductId,

            ProductAttributeId =
                dto.ProductAttributeId,

            Value = dto.Value,

            ParentVariantId =
                dto.ParentVariantId,

            Price = dto.Price,

            Quantity = dto.Quantity,

            CreatedAt = DateTime.UtcNow
        };


        _db.ProductVariants.Add(variant);

        await _db.SaveChangesAsync();

        return Ok(
            "Variant added successfully"
            .SendResponse());
    }


    // ============================================================
    // PUT: Update variant
    // ============================================================
    [HttpPut]
    public async Task<IActionResult> Update(
        VendorProductVariantUpdateDto dto)
    {
        var variant = await _db.ProductVariants
            .FirstOrDefaultAsync(v =>
                v.ProductVariantId ==
                    dto.ProductVariantId);

        if (variant == null)
        {
            return NotFound(
                "Variant not found"
                .SendResponse());
        }


        // --------------------------------------------------------
        // Parent validation
        // --------------------------------------------------------

        if (dto.ParentVariantId.HasValue)
        {
            // Cannot be its own parent
            if (dto.ParentVariantId.Value ==
                dto.ProductVariantId)
            {
                return BadRequest(
                    "A variant cannot be its own parent"
                    .SendResponse());
            }


            var parentVariant = await _db.ProductVariants
                .FirstOrDefaultAsync(v =>
                    v.ProductVariantId ==
                        dto.ParentVariantId.Value &&

                    v.ProductId ==
                        variant.ProductId);


            if (parentVariant == null)
            {
                return BadRequest(
                    "Invalid parent variant"
                    .SendResponse());
            }


            // Child cannot become parent
            if (parentVariant.ParentVariantId.HasValue)
            {
                return BadRequest(
                    "A child variant cannot be used as a parent"
                    .SendResponse());
            }
        }


        // --------------------------------------------------------
        // Duplicate check
        // --------------------------------------------------------

        var exists = await _db.ProductVariants
            .AnyAsync(v =>
                v.ProductVariantId !=
                    dto.ProductVariantId &&

                v.ProductId ==
                    variant.ProductId &&

                v.ParentVariantId ==
                    dto.ParentVariantId &&

                v.ProductAttributeId ==
                    variant.ProductAttributeId &&

                v.Value ==
                    dto.Value);


        if (exists)
        {
            return BadRequest(
                "Variant already exists"
                .SendResponse());
        }


        // --------------------------------------------------------
        // Update
        // --------------------------------------------------------

        variant.ParentVariantId =
            dto.ParentVariantId;

        variant.Value =
            dto.Value;

        variant.Price =
            dto.Price;

        variant.Quantity =
            dto.Quantity;

        variant.ModifiedAt =
            DateTime.UtcNow;


        await _db.SaveChangesAsync();

        return Ok(
            "Variant updated"
            .SendResponse());
    }


    // ============================================================
    // DELETE: Remove variant
    // ============================================================
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var variant = await _db.ProductVariants
            .FirstOrDefaultAsync(v =>
                v.ProductVariantId == id);

        if (variant == null)
            return NotFound(
                "Variant not found"
                .SendResponse());


        // --------------------------------------------------------
        // Check child variants
        // --------------------------------------------------------

        var hasChildren = await _db.ProductVariants
            .AnyAsync(v =>
                v.ParentVariantId == id);


        if (hasChildren)
        {
            return BadRequest(
                "Cannot delete this variant because it has child variants"
                .SendResponse());
        }


        // --------------------------------------------------------
        // Delete
        // --------------------------------------------------------

        _db.ProductVariants.Remove(variant);

        await _db.SaveChangesAsync();

        return Ok(
            "Variant deleted"
            .SendResponse());
    }
}