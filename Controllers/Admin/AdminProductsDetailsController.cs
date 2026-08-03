using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorEcommerceProject.Dtos.Admin.Products;
using VendorEcommerceProject.Helpers;

[ApiController]
[Route("api/admin/products")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminProductsDetailsController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminProductsDetailsController(AppDbContext db)
    {
        _db = db;
    }

    // ----------------------------------------------------
    // GET: Full product details (Admin review)
    // ----------------------------------------------------
    [HttpGet("{id:long}/details")]
    public async Task<IActionResult> GetProductDetails(long id)
    {
        var product = await _db.Products
            //.Include(p => p.Vendor)
            .Include(p => p.Category)
            .Include(p => p.Status)
            .Include(p => p.ProductImages)
            .Include(p => p.ProductTags)
            .Include(p => p.Variants)
                .ThenInclude(v => v.Attribute)
            .FirstOrDefaultAsync(p => p.ProductId == id);

        if (product == null)
            return NotFound("Product not found".SendResponse());

        var baseUri = $"{Request.Scheme}://{Request.Host}";


        var dto = new AdminProductDetailsDto
        {
            ProductId = product.ProductId,
            ProductsName = product.ProductsName,
            Description = product.Description,
            Sku = product.Sku,
            Price = product.Price,
            Quantity = product.Quantity,
            StatusName = product.Status.Name,

            //VendorId = product.VendorId,
            //VendorName = product.Vendor.Name,

            CategoryName = product.Category.Name,



            Images = product.ProductImages
                .Select(i => new AdminProductImageDto
                {
                    ProductImageId = i.ProductImageId,
                    ImageUrl = $"{baseUri}{i.ImageUrl}"
                })
                .ToList(),

            Variants = product.Variants
                .Select(v => new AdminProductVariantDto
                {
                    ProductVariantId = v.ProductVariantId,
                    AttributeName = v.Attribute.Name,
                    Value = v.Value,
                    Price = v.Price,
                    Quantity = v.Quantity
                })
                .ToList(),

            Tags = product.ProductTags
                .Select(t => new AdminProductTagDto
                {
                    ProductTagId = t.ProductTagId,
                    Name = t.Name
                })
                .ToList()
        };

        return Ok(dto);
    }
}
