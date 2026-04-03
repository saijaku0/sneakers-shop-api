using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Application.Auth.DTOs
{
    /// <summary>
    /// Represents a request to generate an authentication token for a user, including user identification, email, and
    /// assigned roles. 
    /// </summary>
    /// <param name="UserId">The unique identifier of the user for whom the token is being generated.</param>
    /// <param name="Email">The email address associated with the user. Cannot be null or empty.</param>
    /// <param name="Roles">A list of roles assigned to the user. Determines the permissions included in the generated token. Cannot be
    /// null.</param>
    public record TokenGenerationRequest(
        Guid UserId,
        string Email,
        IList<UserRole> Roles);
}
