using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorEcommerceProject.Dtos.Admin.ProductTags;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models.ProductsTables;

[ApiController]
[Route("api/admin/product-tags")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminProductTagsController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminProductTagsController(AppDbContext db)
    {
        _db = db;
    }

    // GET: all tags
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tags = await _db.ProductTags
            .Select(t => new AdminProductTagListDto
            {
                ProductTagId = t.ProductTagId,
                Name = t.Name
            })
            .ToListAsync();

        return Ok(tags);
    }

    // POST: create tag
    [HttpPost]
    public async Task<IActionResult> Create(AdminProductTagCreateDto dto)
    {
        if (await _db.ProductTags.AnyAsync(t => t.Name == dto.Name))
            return BadRequest("Tag already exists".SendResponse());

        var tag = new ProductTag
        {
            Name = dto.Name,
            CreatedAt = DateTime.UtcNow
        };

        _db.ProductTags.Add(tag);
        await _db.SaveChangesAsync();

        return Ok("Tag created".SendResponse());
    }

    // DELETE: delete tag (safe)
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        bool usedByProduct = await _db.Products
            .AnyAsync(p => p.ProductTags.Any(t => t.ProductTagId == id));

        if (usedByProduct)
            return BadRequest("Tag is used by products".SendResponse());

        var tag = await _db.ProductTags.FindAsync(id);
        if (tag == null)
            return NotFound();

        _db.ProductTags.Remove(tag);
        await _db.SaveChangesAsync();

        return Ok("Tag deleted".SendResponse());
    }
}
