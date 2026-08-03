using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VendorEcommerceProject.Models.UserDetailsTable;

namespace VendorEcommerceProject.Data;
public static class DbInitializer
{
    public static async Task SeedSuperAdminsAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        // Ensure role exists
        if (!await roleManager.RoleExistsAsync("SuperAdmin"))
            return;

        string defaultPassword = "Admin@123";

        for (int i = 1; i <= 3; i++)
        {
            string username = $"superadmin{i}";
            string email = $"admin{i}@system.com";

            if (await userManager.FindByNameAsync(username) != null)
                continue;

            var user = new Users
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true,
                FullName = $"System Super Admin {i}",
                IsActive = true

            };

            var result = await userManager.CreateAsync(user, defaultPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, "SuperAdmin");
        }
    }
}
