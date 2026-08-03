using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorEcommerceProject.Dtos.Admin.Categories;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models.ProductsTables;

[ApiController]
[Route("api/admin/categories")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminCategoriesController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminCategoriesController(AppDbContext db)
    {
        _db = db;
    }


    // ------------------------------------
    // GET: All Categories tree wise data
    // ------------------------------------
    [HttpGet("tree")]
    public IActionResult GetCategoryTree()
    {
        var categories = _db.ProductCategories
            .Select(c => new
            {
                c.ProductCategoryId,
                c.Name,
                c.ParentId
            })
            .ToList();

        var lookup = categories.ToLookup(c => c.ParentId);

        List<CategoryTreeDto> BuildTree(long? parentId)
        {
            return lookup[parentId]
                .Select(c => new CategoryTreeDto
                {
                    ProductCategoryId = c.ProductCategoryId,
                    Name = c.Name,
                    Children = BuildTree(c.ProductCategoryId)
                })
                .ToList();
        }

        var tree = BuildTree(null);
        return Ok(tree);
    }


    // ------------------------------------
    // GET: All Categories (Flat List)
    // ------------------------------------
    [HttpGet]
    public IActionResult GetAll()
    {
        var categories = _db.ProductCategories
            .Select(c => new AdminCategoryListDto
            {
                ProductCategoryId = c.ProductCategoryId,
                Name = c.Name,
                ParentId = c.ParentId
            })
            .ToList();

        return Ok(categories);
    }

   
   
    [HttpPost]
    public IActionResult Create(AdminCategoryCreateDto dto)
    {
        //if (_db.ProductCategories.Any(c => c.Name == dto.Name))
        //    return BadRequest("Category already exists".SendResponse());

        if (_db.ProductCategories.Any(c => c.ParentId == dto.ParentId &&
                                   c.Name.Trim().ToLower() == dto.Name.Trim().ToLower()))

        {
            return BadRequest("Category already exists".SendResponse());
        }



        if (dto.ParentId.HasValue &&
            !_db.ProductCategories.Any(c => c.ProductCategoryId == dto.ParentId))
            return BadRequest("Parent category not found".SendResponse());

        var category = new ProductCategory
        {
            Name = dto.Name,
            ParentId = dto.ParentId
        };

        _db.ProductCategories.Add(category);
        _db.SaveChanges();

        return Ok();
    }

    // ------------------------------------
    // PUT: Update Category
    // ------------------------------------
    [HttpPut]
    public IActionResult Update(AdminCategoryUpdateDto dto)
    {
        var category = _db.ProductCategories
            .FirstOrDefault(c => c.ProductCategoryId == dto.ProductCategoryId);

        if (category == null)
            return NotFound("Category not found".SendResponse());

        category.Name = dto.Name;
        category.ParentId = dto.ParentId;

        _db.SaveChanges();
        return Ok();
    }

    // ------------------------------------
    // DELETE: Delete Category (SAFE)
    // ------------------------------------
    [HttpDelete("{id:long}")]
    public IActionResult Delete(long id)
    {
        var category = _db.ProductCategories
            .FirstOrDefault(c => c.ProductCategoryId == id);

        if (category == null)
            return NotFound();

        // ❌ If category has child categories
        bool hasChildren = _db.ProductCategories.Any(c => c.ParentId == id);
        if (hasChildren)
            return BadRequest("Cannot delete category with child categories".SendResponse());

        // ❌ If category is used by products
        bool usedByProduct = _db.Products.Any(p => p.CategoryId == id);
        if (usedByProduct)
            return BadRequest("Cannot delete category used by products".SendResponse());

        _db.ProductCategories.Remove(category);
        _db.SaveChanges();

        return Ok();
    }
}
