using System.ComponentModel.DataAnnotations;

namespace VendorEcommerceProject.Dtos.Vendor.ProductTags
{
    public class VendorProductTagAssignDto
    {
        [Required]
        public long ProductId { get; set; }

        [Required]
        public IList<long> ProductTagIds { get; set; } = new List<long>();
    }
}
