namespace Sneakers.Shop.Backend.Api.DTOs
{
    /// <summary>
    /// Represents a request to obtain a new access token using a refresh token.
    /// </summary>
    /// <param name="RefreshToken">The refresh token issued by the authentication server. Cannot be null or empty.</param>
    public record RefreshTokenRequest(string RefreshToken);
}
