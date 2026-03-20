using Microsoft.AspNetCore.Identity;

namespace Sneakers.Shop.Backend.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenExpiryTime { get; set; }
        public DateTimeOffset? LastUpdatedAt { get; set; }
    }
}
