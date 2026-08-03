using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorEcommerceProject.Dtos.Admin.ProductAttributes;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models.ProductsTables;

[ApiController]
[Route("api/admin/product-attributes")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminProductAttributesController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminProductAttributesController(AppDbContext db)
    {
        _db = db;
    }

    // GET: all attributes
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var attributes = await _db.ProductAttributes
            .OrderBy(a => a.SortOrder)
            .Select(a => new AdminProductAttributeListDto
            {
                ProductAttributeId = a.ProductAttributeId,
                Name = a.Name,
                Description = a.Description,
                SortOrder = a.SortOrder
            })
            .ToListAsync();

        return Ok(attributes);
    }

    // POST: create attribute
    [HttpPost]
    public async Task<IActionResult> Create(AdminProductAttributeCreateDto dto)
    {
        if (await _db.ProductAttributes.AnyAsync(a => a.Name == dto.Name))
            return BadRequest("Attribute already exists".SendResponse());

        var attribute = new ProductAttribute
        {
            Name = dto.Name,
            Description = dto.Description,
            SortOrder = dto.SortOrder,
            CreatedAt = DateTime.UtcNow
        };

        _db.ProductAttributes.Add(attribute);
        await _db.SaveChangesAsync();

        return Ok("Attribute created successfully".SendResponse());
    }

    // PUT: update attribute
    [HttpPut]
    public async Task<IActionResult> Update(AdminProductAttributeUpdateDto dto)
    {
        var attribute = await _db.ProductAttributes
            .FirstOrDefaultAsync(a => a.ProductAttributeId == dto.ProductAttributeId);

        if (attribute == null)
            return NotFound("Attribute not found".SendResponse());

        attribute.Name = dto.Name;
        attribute.Description = dto.Description;
        attribute.SortOrder = dto.SortOrder;
        attribute.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok("Attribute updated".SendResponse());
    }

    // DELETE: safe delete
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        bool usedByVariant = await _db.ProductVariants
            .AnyAsync(v => v.ProductAttributeId == id);

        if (usedByVariant)
            return BadRequest("Attribute is used by variants".SendResponse());

        var attribute = await _db.ProductAttributes.FindAsync(id);
        if (attribute == null)
            return NotFound();

        _db.ProductAttributes.Remove(attribute);
        await _db.SaveChangesAsync();

        return Ok("Attribute deleted".SendResponse());
    }
}
