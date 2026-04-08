using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Infrastructure.Auth.Requirments;
using Sneakers.Shop.Backend.Infrastructure.Identity;
using System.Security.Claims;

namespace Sneakers.Shop.Backend.Infrastructure.Auth.Handlers
{
    public class ActiveModeratorHandler (UserManager<ApplicationUser> userManager)
        : AuthorizationHandler<ActiveModeratorRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ActiveModeratorRequirement requirement)
        {
            if(!context.User.IsInRole(nameof(UserRole.Moderator))) return;

            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !user.EmailConfirmed) return;

            context.Succeed(requirement);
        }
    }
}
