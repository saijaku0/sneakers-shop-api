using Sneakers.Shop.Backend.Domain.ValueObjects;

namespace Sneakers.Shop.Backend.Application.Order.DTOs
{
    public record OrderDetailsDto(
        Guid Id,
        DateTimeOffset OrderDate,
        string Status,
        decimal TotalPrice,
        Address ShippingAddress,
        IReadOnlyList<OrderItemDto> Items);
}
