using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorEcommerceProject.Dtos.Admin.Users;
using VendorEcommerceProject.Helpers;

namespace VendorEcommerceProject.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/customers")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminCustomersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminCustomersController(AppDbContext db)
        {
            _db = db;
        }

        // ===============================
        // GET: Customer list
        // ===============================
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _db.Users
                .Where(u => u.UserRoles.Any(r => r.Role.Name == "Customer"))
                .Include(u => u.CustomerDetails)
                .Select(u => new AdminCustomerListDto
                {
                    UserId = u.Id,
                    FullName = u.FullName,
                    Email = u.Email!,
                    IsActive = u.IsActive,
                    PhoneNumber = u.CustomerDetails!.PhoneNumber,
                    Gender = u.CustomerDetails!.Gender
                })
                .ToListAsync();

            return Ok(customers);
        }

        // ===============================
        // PUT: Block / Unblock customer
        // ===============================
        [HttpPut("status")]
        public async Task<IActionResult> UpdateCustomerStatus(AdminUserStatusUpdateDto dto)
        {
            var user = await _db.Users.FindAsync(dto.UserId);
            if (user == null)
                return NotFound("User not found".SendResponse());

            user.IsActive = dto.IsActive;

            await _db.SaveChangesAsync();
            return Ok("Customer status updated".SendResponse());
        }
    }
}
