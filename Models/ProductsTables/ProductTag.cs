using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.ProductsTables
{
    public class ProductTag : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductTagId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public IList<Products> Products { get; set; } = new List<Products>();
    }
}
