using EcommerceProject.Dtos.Admin.PaymentMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorEcommerceProject.Dtos.Admin.PaymentMethods;
using VendorEcommerceProject.Helpers;
using VendorEcommerceProject.Models.PaymentsTable;

namespace EcommerceProject.Controllers.Admin.Payment
{
    [ApiController]
    [Route("api/admin/payment-methods")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminPaymentMethodsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminPaymentMethodsController(AppDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var paymentMethods = await _db.PaymentMethods
                .Select(p => new
                {
                    p.PaymentMethodId,
                    p.Name,
                    p.IsActive,
                    p.CreatedAt,
                    p.ModifiedAt
                })
                .ToListAsync();

            return Ok(paymentMethods);
        }


        [HttpPost]
        public async Task<IActionResult> Create(AdminPaymentMethodCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Payment method name is required".SendResponse());

            var name = dto.Name.Trim();

            bool exists = await _db.PaymentMethods
                .AnyAsync(p => p.Name.ToLower() == name.ToLower());

            if (exists)
                return BadRequest("Payment method already exists".SendResponse());

            var paymentMethod = new PaymentMethod
            {
                Name = name,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.PaymentMethods.Add(paymentMethod);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                paymentMethodId = paymentMethod.PaymentMethodId,
                message = "Payment method created successfully"
            });
        }



        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(
    long id,
    AdminPaymentMethodUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Payment method name is required".SendResponse());

            var paymentMethod = await _db.PaymentMethods
                .FirstOrDefaultAsync(p => p.PaymentMethodId == id);

            if (paymentMethod == null)
                return NotFound("Payment method not found".SendResponse());

            var name = dto.Name.Trim();

            bool exists = await _db.PaymentMethods
                .AnyAsync(p =>
                    p.PaymentMethodId != id &&
                    p.Name.ToLower() == name.ToLower());

            if (exists)
                return BadRequest("Payment method already exists".SendResponse());

            paymentMethod.Name = name;
            paymentMethod.IsActive = dto.IsActive;
            paymentMethod.ModifiedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Payment method updated successfully"
            });
        }
    }


}
