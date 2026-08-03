using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VendorEcommerceProject.Dtos.Auth;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models.UserDetailsTable;
using VendorEcommerceProject.Models.VendorsTable;

namespace VendorEcommerceProject.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthController(
            UserManager<Users> userManager,
            RoleManager<Role> roleManager,
            AppDbContext db,
            IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _config = config;
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var user = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u =>
                    u.Email == dto.EmailOrUsername ||
                    u.UserName == dto.EmailOrUsername);

            if (user == null)
                return Unauthorized("Invalid credentials".SendResponse());

            if (!user.IsActive)
                return Unauthorized("Account is disabled".SendResponse());

            var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!validPassword)
                return Unauthorized("Invalid credentials".SendResponse());

            var roles = user.UserRoles.Select(r => r.Role.Name!).ToList();

            var token = GenerateJwtToken(user, roles);

            return Ok(new LoginResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                Roles = roles,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(6)
            });
        }

        // =====================================================
        // REGISTER CUSTOMER
        // =====================================================
        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerDto dto)
        {
            var user = new Users
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await AssignRole(user, "Customer");

            _db.CustomerDetails.Add(new CustomerDetails
            {
                UserId = user.Id,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender
            });

            await _db.SaveChangesAsync();
            return Ok("Customer registered successfully".SendResponse());
        }

        //// =====================================================
        //// REGISTER VENDOR
        //// =====================================================
        //[HttpPost("register-vendor")]
        //public async Task<IActionResult> RegisterVendor(RegisterVendorDto dto)
        //{
        //    var user = new Users
        //    {
        //        UserName = dto.Email,
        //        Email = dto.Email,
        //        FullName = dto.FullName,
        //        IsActive = true
        //    };

        //    var result = await _userManager.CreateAsync(user, dto.Password);
        //    if (!result.Succeeded)
        //        return BadRequest(result.Errors);

        //    await AssignRole(user, "Vendor");

        //    _db.VendorDetails.Add(new VendorDetails
        //    {
        //        UserId = user.Id,
        //        ShopName = dto.ShopName,
        //        BusinessRegNo = dto.BusinessRegNo,
        //        TaxId = dto.TaxId
        //    });

        //    _db.Vendors.Add(new Vendor
        //    {
        //        UserId = user.Id,
        //        Name = dto.VendorDisplayName,
        //        Description = dto.Description ?? string.Empty
        //    });

        //    await _db.SaveChangesAsync();
        //    return Ok("Vendor registered successfully".SendResponse());
        //}

        // =====================================================
        // REGISTER ADMIN (SUPERADMIN ONLY)
        // =====================================================
        [HttpPost("register-admin")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminDto dto)
        {
            var user = new Users
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await AssignRole(user, "Admin");

            _db.SuperAdminDetails.Add(new SuperAdminDetails
            {
                UserId = user.Id,
                Notes = dto.Notes
            });

            await _db.SaveChangesAsync();
            return Ok("Admin registered successfully".SendResponse());
        }

        // =====================================================
        // HELPERS
        // =====================================================
        private async Task AssignRole(Users user, string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                throw new Exception($"Role '{roleName}' not found");

            _db.UserRoleAssignments.Add(new UserRoleAssignment
            {
                UserId = user.Id,
                RoleId = role.Id
            });
        }

        private string GenerateJwtToken(Users user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("fullName", user.FullName)
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        //====================================================
        // become vendor
        //====================================================


        //[Authorize(Roles = "Customer")]
        //[HttpPost("become-vendor")]
        //public async Task<IActionResult> BecomeVendor(BecomeVendorDto dto)
        //{
        //    //GET USER ID SAFELY FROM JWT
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        //    if (userIdClaim == null)
        //        return Unauthorized("UserId claim missing in token".SendResponse());

        //    if (!long.TryParse(userIdClaim.Value, out var userId))
        //        return Unauthorized("Invalid UserId in token".SendResponse());

        //    // LOAD USER WITH ROLES
        //    var user = await _db.Users
        //        .Include(u => u.UserRoles)
        //            .ThenInclude(ur => ur.Role)
        //        .FirstOrDefaultAsync(u => u.Id == userId);

        //    if (user == null)
        //        return Unauthorized("User not found".SendResponse());

        //    //CHECK IF ALREADY VENDOR

        //    bool isAlreadyVendor = user.UserRoles
        //        .Any(r => r.Role.Name == "Vendor");

        //    if (isAlreadyVendor)
        //        return BadRequest("User is already a vendor".SendResponse());

        //    //ADD VENDOR ROLE
        //    var vendorRole = await _db.Roles
        //        .FirstOrDefaultAsync(r => r.Name == "Vendor");

        //    if (vendorRole == null)
        //        return StatusCode(500, "Vendor role not found".SendResponse());

        //    _db.UserRoleAssignments.Add(new UserRoleAssignment
        //    {
        //        UserId = userId,
        //        RoleId = vendorRole.Id
        //    });

        //    //PREVENT DUPLICATE VENDOR DETAILS
        //    bool hasVendorDetails = await _db.VendorDetails
        //        .AnyAsync(v => v.UserId == userId);

        //    if (!hasVendorDetails)
        //    {
        //        _db.VendorDetails.Add(new VendorDetails
        //        {
        //            UserId = userId,
        //            ShopName = dto.ShopName,
        //            BusinessRegNo = dto.BusinessRegNo,
        //            TaxId = dto.TaxId
        //        });
        //    }

        //    //PREVENT DUPLICATE VENDOR ACCOUNT
        //    bool hasVendorAccount = await _db.Vendors
        //        .AnyAsync(v => v.UserId == userId);

        //    if (!hasVendorAccount)
        //    {
        //        _db.Vendors.Add(new Vendor
        //        {
        //            UserId = userId,
        //            Name = dto.VendorDisplayName,
        //            Description = dto.Description ?? string.Empty
        //        });
        //    }
        //    //SAVE CHANGES
        //    await _db.SaveChangesAsync();

        //    return Ok("You are now a vendor".SendResponse());
        //}


    }
}
