using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorEcommerceProject.Models.UserDetailsTable;
using VendorEcommerceProject.Data;

namespace VendorEcommerceProject.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        // =====================================================
        // GET ALL USERS (WITH ROLES)
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _db.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.UserName,
                    u.IsActive,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }
    }
}
