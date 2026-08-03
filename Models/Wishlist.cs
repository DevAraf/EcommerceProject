using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendorEcommerceProject.Models.ProductsTables;
using VendorEcommerceProject.Models.UserDetailsTable;

namespace VendorEcommerceProject.Models
{
    public class Wishlist : BaseEntity
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users User { get; set; } = null!;

        [Required]
        public long ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Products Product { get; set; } = null!;
    }

    //modelBuilder.Entity<Wishlist>()
    //.HasKey(w => new { w.UserId, w.ProductId});

}
