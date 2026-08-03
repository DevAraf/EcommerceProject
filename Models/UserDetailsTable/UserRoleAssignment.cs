using System.ComponentModel.DataAnnotations.Schema;

namespace VendorEcommerceProject.Models.UserDetailsTable
{
    public class UserRoleAssignment
    {
        public long UserId { get; set; }
        public Users User { get; set; } = null!;

        public long RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }

    //modelBuilder.Entity<UserRoleAssignment>()
    //.HasKey(x => new { x.UserId, x.RoleId});

}
