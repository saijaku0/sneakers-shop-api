namespace Sneakers.Shop.Backend.Application.Order.DTOs
{
    public record OrderItemDto(
        Guid WarehouseItemId,
        string ProductName,
        decimal UnitPrice,
        decimal DiscountAmount,
        int Quantity,
        decimal TotalPrice);
}
