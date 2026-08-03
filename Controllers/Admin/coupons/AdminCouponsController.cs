using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorEcommerceProject.Dtos.Admin.Coupons;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models.OrdersAndCartTable;

[ApiController]
[Route("api/admin/coupons")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminCouponsController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminCouponsController(AppDbContext db)
    {
        _db = db;
    }

    // ----------------------------------------
    // GET: All coupons
    // ----------------------------------------
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var coupons = await _db.Coupons
            .Select(c => new AdminCouponListDto
            {
                CouponId = c.CouponId,
                Code = c.Code,
                Discount = c.Discount,
                Type = c.Type,
                ValidFrom = c.ValidFrom,
                ValidTo = c.ValidTo,
                UsageLimit = c.UsageLimit
            })
            .ToListAsync();

        return Ok(coupons);
    }

    // ----------------------------------------
    // POST: Create coupon
    // ----------------------------------------
    [HttpPost]
    public async Task<IActionResult> Create(AdminCouponCreateDto dto)
    {
        if (await _db.Coupons.AnyAsync(c => c.Code == dto.Code))
            return BadRequest("Coupon code already exists".SendResponse());

        var coupon = new Coupon
        {
            Code = dto.Code.ToUpper(),
            Discount = dto.Discount,
            Type = dto.Type,
            ValidFrom = dto.ValidFrom,
            ValidTo = dto.ValidTo,
            UsageLimit = dto.UsageLimit,
            CreatedAt = DateTime.UtcNow
        };

        _db.Coupons.Add(coupon);
        await _db.SaveChangesAsync();

        return Ok("Coupon created successfully".SendResponse());
    }

    // ----------------------------------------
    // PUT: Update coupon
    // ----------------------------------------
    [HttpPut]
    public async Task<IActionResult> Update(AdminCouponUpdateDto dto)
    {
        var coupon = await _db.Coupons
            .FirstOrDefaultAsync(c => c.CouponId == dto.CouponId);

        if (coupon == null)
            return NotFound("Coupon not found".SendResponse());

        coupon.Discount = dto.Discount;
        coupon.Type = dto.Type;
        coupon.ValidFrom = dto.ValidFrom;
        coupon.ValidTo = dto.ValidTo;
        coupon.UsageLimit = dto.UsageLimit;
        coupon.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok("Coupon updated successfully".SendResponse());
    }

    // ----------------------------------------
    // DELETE: Delete coupon (safe)
    // ----------------------------------------
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        bool usedInOrders = await _db.Orders
            .AnyAsync(o => o.CouponId == id);

        if (usedInOrders)
            return BadRequest("Coupon already used in orders".SendResponse());

        var coupon = await _db.Coupons.FindAsync(id);
        if (coupon == null)
            return NotFound();

        _db.Coupons.Remove(coupon);
        await _db.SaveChangesAsync();

        return Ok("Coupon deleted".SendResponse());
    }
}
