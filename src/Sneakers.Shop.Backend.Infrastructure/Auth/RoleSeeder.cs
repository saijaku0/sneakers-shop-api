using Microsoft.AspNetCore.Identity;
using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Infrastructure.Auth
{
    public class RoleSeeder(RoleManager<IdentityRole<Guid>> roleManager)
    {
        public async Task SeedAsync()
        {
            foreach (var role in Enum.GetNames<UserRole>())
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }
}
