using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorEcommerceProject.Dtos.Admin.ProductsStatusDtos;
using VendorEcommerceProject.Helpers;

[ApiController]
[Route("api/admin/products")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminProductsStatusController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminProductsStatusController(AppDbContext db)
    {
        _db = db;
    }

    // ------------------------------------------------
    // GET: All Products (Any Status)
    // ------------------------------------------------
    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = _db.Products
            .Select(p => new AdminProductListDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductsName,
                //VendorName = p.Vendor.Name,
                CategoryName = p.Category.Name,
                StatusName = p.Status.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                CreatedAt = p.CreatedAt
            })
            .ToList();

        return Ok(products);
    }

    // ------------------------------------------------
    // GET: Single Product Details (Admin View)
    // ------------------------------------------------
    [HttpGet("{id:long}")]
    public IActionResult GetProductDetails(long id)
    {
        var product = _db.Products
            .Where(p => p.ProductId == id)
            .Select(p => new AdminProductDetailsDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductsName,
                Description = p.Description,
                Price = p.Price,
                Quantity = p.Quantity,
                Sku = p.Sku,
                //VendorName = p.Vendor.Name,
                CategoryName = p.Category.Name,
                ProductStatusId = p.ProductStatusId,
                StatusName = p.Status.Name
            })
            .FirstOrDefault();

        if (product == null)
            return NotFound("Product not found".SendResponse());

        return Ok(product);
    }

    // ------------------------------------------------
    // PUT: Change Product Status (Approve / Reject / Block / Pending)
    // ------------------------------------------------
    [HttpPut("change-status")]
    public IActionResult ChangeProductStatus(AdminProductStatusUpdateDto dto)
    {
        var product = _db.Products.FirstOrDefault(p => p.ProductId == dto.ProductId);
        if (product == null)
            return NotFound("Product not found".SendResponse());

        bool statusExists = _db.ProductStatuses
            .Any(s => s.ProductStatusId == dto.ProductStatusId);

        if (!statusExists)
            return BadRequest("Invalid product status".SendResponse());

        product.ProductStatusId = dto.ProductStatusId;
        product.ModifiedAt = DateTime.UtcNow;

        _db.SaveChanges();
        return Ok("Product status updated successfully".SendResponse());
    }
}
