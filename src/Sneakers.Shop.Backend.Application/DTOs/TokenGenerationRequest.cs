using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Application.DTOs
{
    public record TokenGenerationRequest(
        Guid UserId,
        string Email,
        IList<UserRole> Roles);
}
