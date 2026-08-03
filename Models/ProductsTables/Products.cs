using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendorEcommerceProject.Models.OrdersAndCartTable;
using VendorEcommerceProject.Models.VendorsTable;

namespace VendorEcommerceProject.Models.ProductsTables
{
    public class Products : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductId { get; set; }

        //[Required]
        //public long VendorId { get; set; }

        //[ForeignKey(nameof(VendorId))]
        //public Vendor Vendor { get; set; } = null!;

        [Required]
        public long CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public ProductCategory Category { get; set; } = null!;

        [Required]
        public long ProductStatusId { get; set; }

        [ForeignKey(nameof(ProductStatusId))]
        public ProductStatus Status { get; set; } = null!;

        [Required, MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string ProductsName { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required, MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Sku { get; set; } = string.Empty;

        public IList<ProductsImages> ProductImages { get; set; } = new List<ProductsImages>();
        public IList<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public IList<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
        public IList<CustomersReview> CustomerReviews { get; set; } = new List<CustomersReview>();
        public IList<ProductsReview> ProductReviews { get; set; } = new List<ProductsReview>();
        public IList<CartItem> CartItems { get; set; } = new List<CartItem>();
        public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
