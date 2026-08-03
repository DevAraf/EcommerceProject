using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VendorEcommerceProject.Models.UserDetailsTable
{
    public class Role : IdentityRole<long>
    {
        [MaxLength(500)]
        public string? Description { get; set; }

        public IList<UserRoleAssignment> UserRoles { get; set; } = new List<UserRoleAssignment>();
    }
}
