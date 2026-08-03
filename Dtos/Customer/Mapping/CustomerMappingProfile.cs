//using VendorEcommerceProject.Dtos.Customer.Cart;
//using VendorEcommerceProject.Dtos.Customer.Orders;
//using VendorEcommerceProject.Dtos.Customer.Profile;
//using VendorEcommerceProject.Dtos.Customer.Wishlist;
//using VendorEcommerceProject.Models.OrdersAndCartTable;
//using VendorEcommerceProject.Models.UserDetailsTable;

//namespace VendorEcommerceProject.Dtos.Customer.Mapping
//{
//    public class CustomerMappingProfile
//    {
//        public CustomerMappingProfile()
//        {
//            // -----------------------
//            // Profile
//            // -----------------------
//            CreateMap<CustomerDetails, CustomerProfileDto>();
//            CreateMap<UserAddress, CustomerAddressDto>();

//            // -----------------------
//            // Cart → CartItems grouped by Vendor
//            // -----------------------
//            CreateMap<Cart, CustomerCartDto>()
//                .ForMember(dest => dest.Vendors, opt => opt.MapFrom(src =>
//                    src.Items
//                       .GroupBy(ci => ci.Product.Vendor)
//                       .Select(g => new CartVendorDto
//                       {
//                           VendorId = g.Key.VendorId,
//                           VendorName = g.Key.VendorName,
//                           Items = g.Select(ci => new CartItemDto
//                           {
//                               ProductId = ci.ProductId,
//                               ProductName = ci.Product.ProductsName,
//                               Quantity = ci.Quantity,
//                               UnitPrice = ci.Product.Price,
//                               TotalPrice = ci.Quantity * ci.Product.Price
//                           }).ToList(),
//                           SubTotal = g.Sum(ci => ci.Quantity * ci.Product.Price)
//                       }).ToList()));

//            CreateMap<CartItem, CartItemDto>()
//                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductsName))
//                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.Price))
//                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Quantity * src.Product.Price));

//            // -----------------------
//            // Orders → OrderItems + Shipment info
//            // -----------------------
//            CreateMap<Orders, CustomerOrderListDto>()
//                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.OrderItems.Sum(oi => oi.Quantity * oi.Price)))
//                .ForMember(dest => dest.OrderStatusName, opt => opt.MapFrom(src => src.Status.Name))
//                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems))
//                .ForMember(dest => dest.ShipmentStatus, opt => opt.MapFrom(src =>
//                    src.Shipments.OrderByDescending(s => s.CreatedAt)
//                                 .Select(s => s.Status.Name)
//                                 .FirstOrDefault()))
//                .ForMember(dest => dest.TrackingNumber, opt => opt.MapFrom(src =>
//                    src.Shipments.OrderByDescending(s => s.CreatedAt)
//                                 .Select(s => s.TrackingNumber)
//                                 .FirstOrDefault()));

//            CreateMap<OrderItem, OrderItemDto>()
//                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductsName))
//                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price))
//                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Quantity * src.Price));

//            // -----------------------
//            // Wishlist → first image
//            // -----------------------
//            CreateMap<Wishlist, WishlistItemDto>()
//                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductsName))
//                .ForMember(dest => dest.FirstImageUrl, opt => opt.MapFrom(src =>
//                    src.Product.ProductImages
//                        .OrderBy(pi => pi.ProductImageId)
//                        .Select(pi => pi.ImageUrl)
//                        .FirstOrDefault()));
//        }
//    }
//}
