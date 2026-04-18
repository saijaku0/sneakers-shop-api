namespace Sneakers.Shop.Backend.Application.Reservations.DTOs
{
    public record CartItemDto(
        Guid ReservationId,
        Guid WarehouseItemId,
        Guid ProductId,
        string ProductName,
        decimal Price,
        int Quantity,
        DateTimeOffset ExpiresAt);
}
