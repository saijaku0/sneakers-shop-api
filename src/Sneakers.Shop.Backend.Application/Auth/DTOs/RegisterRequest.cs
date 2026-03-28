namespace Sneakers.Shop.Backend.Application.Auth.DTOs
{
    /// <summary>
    /// Represents a request to register a new user with an email address and password. 
    /// </summary>
    /// <param name="Email">The email address to associate with the new user account. Cannot be null or empty.</param>
    /// <param name="Password">The password to set for the new user account. Cannot be null or empty.</param>
    public record RegisterRequest(string Email, string Password);
}
