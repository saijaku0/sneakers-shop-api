namespace Sneakers.Shop.Backend.Application.DTOs
{
    public record TokenGenerationRequest(
        Guid UserId,
        string Email,
        IList<string> Roles);
}
