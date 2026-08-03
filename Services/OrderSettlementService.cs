using Microsoft.EntityFrameworkCore;
using VendorEcommerceProject.Models.VendorsTable;
using VendorEcommerceProject.Models.OrdersAndCartTable;

namespace VendorEcommerceProject.Services
{
    public class OrderSettlementService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public OrderSettlementService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        /// <summary>
        /// Called ONCE after payment success
        /// </summary>
        public async Task SettleOrderAsync(long orderId)
        {
            // 1️⃣ Load order with items + products + vendors
            var order = await _db.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                throw new Exception("Order not found");

            // 2️⃣ Prevent duplicate settlement
            //bool alreadySettled = await _db.Commissions
            //    .AnyAsync(c => c.OrderId == orderId);

            //if (alreadySettled)
            //    return; // SAFETY EXIT

            // 3️⃣ Commission rate
            decimal commissionRate =
                _config.GetValue<decimal>("Commission:DefaultRate"); // ex: 10

            // 4️⃣ Process each order item
            foreach (var item in order.OrderItems)
            {
                var vendorId = item.Product;
                var itemTotal = item.Price * item.Quantity;

                var commissionAmount = itemTotal * commissionRate / 100;
                var vendorEarning = itemTotal - commissionAmount;

                // 🔹 Commission record
                //_db.Commissions.Add(new Commission
                //{
                //    //VendorId = vendorId,
                //    OrderId = orderId,
                //    Amount = commissionAmount,
                //    CreatedAt = DateTime.UtcNow
                //});

                // 🔹 Vendor earning record
                //_db.VendorEarnings.Add(new VendorEarning
                //{
                //    //VendorId = vendorId,
                //    OrderId = orderId,
                //    Amount = vendorEarning,
                //    Status = "pending",
                //    CreatedAt = DateTime.UtcNow
                //});
            }

            // 5️⃣ Save once (atomic)
            await _db.SaveChangesAsync();
        }
    }
}
