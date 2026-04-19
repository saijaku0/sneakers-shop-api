namespace Sneakers.Shop.Backend.Application.Order.DTOs
{
    public record OrderDto(
        Guid Id,
        DateTimeOffset OrderDate,
        string Status,
        decimal TotalPrice,
        int ItemsCount);
}
