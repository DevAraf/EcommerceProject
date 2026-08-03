using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Controllers.Customer.Payment
{
    [ApiController]
    [Route("api/customer/payment-methods")]
    [Authorize(Roles = "Customer")]
    public class CustomerPaymentMethodsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CustomerPaymentMethodsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetActivePaymentMethods()
        {
            var paymentMethods = await _db.PaymentMethods
                .Where(p => p.IsActive)
                .Select(p => new
                {
                    p.PaymentMethodId,
                    p.Name
                })
                .ToListAsync();

            return Ok(paymentMethods);
        }
    }
}