using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorEcommerceProject.Dtos.Admin.Users;
using VendorEcommerceProject.Helpers;

namespace VendorEcommerceProject.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/vendors")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminVendorsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminVendorsController(AppDbContext db)
        {
            _db = db;
        }

        // ===============================
        // GET: Vendor list
        // ===============================
        //[HttpGet]
        //public async Task<IActionResult> GetVendors()
        //{
        //    var vendors = await _db.Vendors
        //        .Include(v => v.User)
        //        .Include(v => v.User.VendorDetails)
        //        .Select(v => new AdminVendorListDto
        //        {
        //            VendorId = v.VendorId,
        //            UserId = v.UserId,
        //            VendorName = v.Name,
        //            OwnerName = v.User.FullName,
        //            Email = v.User.Email!,
        //            IsActive = v.User.IsActive,
        //            ShopName = v.User.VendorDetails!.ShopName,
        //            BusinessRegNo = v.User.VendorDetails!.BusinessRegNo
        //        })
        //        .ToListAsync();

        //    return Ok(vendors);
        //}

        // ===============================
        // PUT: Block / Unblock vendor
        // ===============================
        [HttpPut("status")]
        public async Task<IActionResult> UpdateVendorStatus(AdminUserStatusUpdateDto dto)
        {
            var user = await _db.Users.FindAsync(dto.UserId);
            if (user == null)
                return NotFound("User not found".SendResponse());

            user.IsActive = dto.IsActive;

            await _db.SaveChangesAsync();
            return Ok("Vendor status updated".SendResponse());
        }
    }
}
