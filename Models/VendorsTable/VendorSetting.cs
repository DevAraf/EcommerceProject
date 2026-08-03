using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.VendorsTable
{
    public class VendorSetting : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VendorSettingId { get; set; }

        [Required]
        public long VendorId { get; set; }

        [ForeignKey(nameof(VendorId))]
        public Vendor Vendor { get; set; } = null!;

        [Required, MaxLength(150)]
        [Column(TypeName = "nvarchar(150)")]
        public string Key { get; set; } = string.Empty;

        [Required, MaxLength(2000)]
        [Column(TypeName = "nvarchar(2000)")]
        public string Value { get; set; } = string.Empty;
    }
}
