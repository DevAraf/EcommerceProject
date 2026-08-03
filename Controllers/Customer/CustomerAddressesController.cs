using EcommerceProject.Dtos.Customer.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendorEcommerceProject.Dtos.Customer.Profile;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models.UserDetailsTable;

namespace VendorEcommerceProject.Controllers.Customer
{
    [Route("api/customer/addresses")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CustomerAddressesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CustomerAddressesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(
            [FromBody] CustomerAddressCreateDto dto)
        {
            long userId = long.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Check if customer already has an address
            bool hasAddress = await _db.UserAddresses
                .AnyAsync(a => a.UserId == userId);

            // First address automatically becomes default
            bool isDefault = !hasAddress || dto.IsDefault;

            // If new address is default,
            // remove default from previous address
            if (isDefault)
            {
                var oldDefaults = await _db.UserAddresses
                    .Where(a => a.UserId == userId && a.IsDefault)
                    .ToListAsync();

                oldDefaults.ForEach(a => a.IsDefault = false);
            }

            var address = new UserAddress
            {
                UserId = userId,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                IsDefault = isDefault
            };

            _db.UserAddresses.Add(address);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                addressId = address.UserAddressId,
                isDefault = address.IsDefault,
                message = "Address added successfully".SendResponse()
            });
        }
    }
}