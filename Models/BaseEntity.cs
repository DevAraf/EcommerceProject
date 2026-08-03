using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
