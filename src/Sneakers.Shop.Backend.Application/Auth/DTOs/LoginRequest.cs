namespace Sneakers.Shop.Backend.Application.Auth.DTOs
{
    /// <summary>
    /// Represents a request to authenticate a user using an email address and password.
    /// </summary>
    /// <param name="Email">The email address associated with the user account to authenticate. Cannot be null or empty.</param>
    /// <param name="Password">The password corresponding to the specified email address. Cannot be null or empty.</param>
    public record LoginRequest(string Email, string Password);
}
