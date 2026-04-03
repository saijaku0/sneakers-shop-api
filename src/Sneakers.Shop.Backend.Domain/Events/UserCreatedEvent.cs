using Sneakers.Shop.Backend.Domain.ValueObjects;

namespace Sneakers.Shop.Backend.Domain.Events
{
    public record UserCreatedEvent(
        Guid UserId,
        string Name,
        string Lastname,
        string PhoneNumber,
        string Email,
        Address? DefaultShippingAddress) : IDomainEvent;
}
