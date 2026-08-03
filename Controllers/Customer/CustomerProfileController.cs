using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models.UserDetailsTable;


[ApiController]
[Route("api/customer/profile")]
[Authorize(Roles = "Customer")]
public class CustomerProfileController : ControllerBase
{
    private readonly AppDbContext _db;

    public CustomerProfileController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Customer basic info
        var profile = await _db.CustomerDetails
            .Include(c => c.User)
            .Where(c => c.UserId == userId)
            .Select(c => new
            {
                UserId = c.UserId,
                FullName = c.User!.FullName,
                Email = c.User.Email,
                PhoneNumber = c.PhoneNumber,
                DateOfBirth = c.DateOfBirth,
                Gender = c.Gender
            })
            .FirstOrDefaultAsync();

        if (profile == null)
            return NotFound("Customer profile not found".SendResponse());

        // Addresses
        var addresses = await _db.UserAddresses
            .Where(a => a.UserId == userId)
            .Select(a => new
            {
                AddressId = a.UserAddressId,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                City = a.City,
                PostalCode = a.PostalCode,
                Country = a.Country,
                IsDefault = a.IsDefault
            })
            .ToListAsync();

        return Ok(new
        {
            profile.UserId,
            profile.FullName,
            profile.Email,
            profile.PhoneNumber,
            profile.DateOfBirth,
            profile.Gender,
            Addresses = addresses
        });
    }
}
