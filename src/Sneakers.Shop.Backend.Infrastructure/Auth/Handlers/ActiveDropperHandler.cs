using Microsoft.AspNetCore.Authorization;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Auth.Requirments;
using System.Security.Claims;

namespace Sneakers.Shop.Backend.Infrastructure.Auth.Handlers
{
    public class ActiveDropperHandler (IUserProfileRepository userProfile)
        : AuthorizationHandler<ActiveDropperRequirement>
    {
        private readonly IUserProfileRepository _userProfile = userProfile;
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            ActiveDropperRequirement requirement)
        {
            if (!context.User.IsInRole(nameof(UserRole.Dropper)))
                return;

            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return;

            var isFlagged = await _userProfile.IsUserFlaggedAsync(Guid.Parse(userId));
            if (!isFlagged)
                context.Succeed(requirement);
        }
    }
}
