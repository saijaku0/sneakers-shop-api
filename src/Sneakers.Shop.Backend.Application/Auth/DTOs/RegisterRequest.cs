using Sneakers.Shop.Backend.Domain.ValueObjects;

namespace Sneakers.Shop.Backend.Application.Auth.DTOs
{
    /// <summary>
    /// Represents a request to register a new user with personal information, credentials, and an optional default
    /// shipping address.
    /// </summary>
    /// <param name="Name">The first name of the user to register. Cannot be null or empty.</param>
    /// <param name="Lastname">The last name of the user to register. Cannot be null or empty.</param>
    /// <param name="PhoneNumber">The phone number associated with the user. Must be a valid phone number format.</param>
    /// <param name="Email">The email address for the user account. Must be a valid email address and cannot be null or empty.</param>
    /// <param name="Password">The password for the new user account. Cannot be null or empty.</param>
    /// <param name="DefaultShippingAddress">The default shipping address for the user. Can be null if no default address is specified.</param>
    public record RegisterRequest(
        string Name,
        string Lastname,
        string PhoneNumber,
        string Email, 
        string Password,
        Address? DefaultShippingAddress);
}
