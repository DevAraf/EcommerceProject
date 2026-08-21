using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendorEcommerceProject.Dtos.Products;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models.ProductsTables;


[ApiController]
[Route("api/products")]
[Authorize(Roles = "Admin")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ProductsController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // --------------------------------------------------
    // GET: Vendor own products
    // --------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> GetMyProducts()
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        //var vendorId = await _db.Vendors
        //    .Where(v => v.UserId == userId)
        //    .Select(v => v.VendorId)
        //    .FirstOrDefaultAsync();

        var baseUri = $"{Request.Scheme}://{Request.Host}";

        var products = await _db.Products
            .Select(p => new ProductListDto
            {
                ProductId = p.ProductId,
                ProductsName = p.ProductsName,
                Price = p.Price,
                Quantity = p.Quantity,
                StatusName = p.Status.Name,
                CreatedAt = p.CreatedAt,

                FirstImageUrl = p.ProductImages
                    .OrderBy(i => i.ProductImageId)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault()
            })
            .ToListAsync();

        foreach (var product in products)
        {
            if (!string.IsNullOrEmpty(product.FirstImageUrl))
            {
                product.FirstImageUrl =
                    $"{baseUri}{product.FirstImageUrl}";
            }
        }

        return Ok(products);
    }

    // --------------------------------------------------
    // GET: Product by ID
    // --------------------------------------------------
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetProductById(long id)
    {
        var product = await _db.Products
            .Include(p => p.ProductImages)
            .Where(p => p.ProductId == id && p.DeletedAt == null)
            .Select(p => new
            {
                productId = p.ProductId,
                productsName = p.ProductsName,
                description = p.Description,
                price = p.Price,
                quantity = p.Quantity,
                sku = p.Sku,
                categoryId = p.CategoryId,
                categoryName = p.Category.Name,

                images = p.ProductImages
                    .Select(i => new
                    {
                        productImageId = i.ProductImageId,
                        imageUrl = i.ImageUrl,
                        fileName = i.FileName
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (product == null)
            return NotFound("Product not found".SendResponse());

        return Ok(product);
    }

    // --------------------------------------------------
    // POST: Create product
    // --------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ProductCreateDto dto)
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        //var vendor = await _db.Vendors.FirstOrDefaultAsync(v => v.UserId == userId);
        //if (vendor == null) return BadRequest("Vendor account not found".SendResponse());

        var ApprovedStatusId = await _db.ProductStatuses
            .Where(s => s.Name == "Approved")
            .Select(s => s.ProductStatusId)
            .FirstAsync();

        var product = new Products
        {
            
            CategoryId = dto.CategoryId,
            ProductsName = dto.ProductsName,
            Description = dto.Description,
            Price = dto.Price,
            Quantity = dto.Quantity,
            Sku = dto.Sku,
            ProductStatusId = ApprovedStatusId,
            CreatedAt = DateTime.UtcNow
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        await SaveImages(product.ProductId, dto.Images);

        return Ok("Product created successfully".SendResponse());
    }

    // --------------------------------------------------
    // PUT: Update product (own only)
    // --------------------------------------------------
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromForm] ProductUpdateDto dto)
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var product = await _db.Products
            .Include(p => p.ProductImages)
            //.Include(p => p.Vendor)
            .FirstOrDefaultAsync(p => p.ProductId == id );

        if (product == null) return NotFound("Product not found".SendResponse());

        product.ProductsName = dto.ProductsName;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Quantity = dto.Quantity;
        product.Sku = dto.Sku;
        product.CategoryId = dto.CategoryId;
        product.ModifiedAt = DateTime.UtcNow;

        // Back to pending after edit
        product.ProductStatusId = await _db.ProductStatuses
            .Where(s => s.Name == "Pending")
            .Select(s => s.ProductStatusId)
            .FirstAsync();

        // delete images
        if (dto.ImageIdsToDelete != null)
        {
            var images = product.ProductImages
                .Where(i => dto.ImageIdsToDelete.Contains(i.ProductImageId))
                .ToList();

            _db.ProductImages.RemoveRange(images);
        }

        await SaveImages(product.ProductId, dto.ImagesToAdd);
        await _db.SaveChangesAsync();

        return Ok("Product updated and re-sent for approval".SendResponse());
    }

    // --------------------------------------------------
    // DELETE: Soft delete product
    // --------------------------------------------------
    //[HttpDelete("{id:long}")]
    //public async Task<IActionResult> SoftDelete(long id)
    //{
    //    long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    //    var product = await _db.Products
    //         //.include(p => p.vendor)
    //         .ToListAsync();

    //    if (product == null) return NotFound();

    //    product.DeletedAt = DateTime.UtcNow;
    //    product.DeletedBy = userId.ToString();

    //    await _db.SaveChangesAsync();
    //    return Ok("Product removed from vendor listing".SendResponse());
    //}
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> SoftDelete(long id)
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var product = await _db.Products
            .FirstOrDefaultAsync(p => p.ProductId == id);

        if (product == null)
            return NotFound();

        product.DeletedAt = DateTime.UtcNow;
        product.DeletedBy = userId.ToString();

        await _db.SaveChangesAsync();

        return Ok("Product removed from vendor listing".SendResponse());
    }

    // --------------------------------------------------
    // IMAGE SAVE HELPER
    // --------------------------------------------------
    private async Task SaveImages(long productId, IList<IFormFile>? images)
    {
        if (images == null || images.Count == 0) return;

        var uploadPath = Path.Combine(_env.WebRootPath, "images", "products");
        Directory.CreateDirectory(uploadPath);

        foreach (var file in images)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            _db.ProductImages.Add(new ProductsImages
            {
                ProductId = productId,
                ImageUrl = $"/images/products/{fileName}",
                FileName = file.FileName,
                ImageFileType = file.ContentType,
                FileSize = file.Length
            });
        }

        await _db.SaveChangesAsync();
    }
}
