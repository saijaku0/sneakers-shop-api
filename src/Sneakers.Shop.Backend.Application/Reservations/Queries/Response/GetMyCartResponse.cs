using Sneakers.Shop.Backend.Application.Reservations.DTOs;

namespace Sneakers.Shop.Backend.Application.Reservations.Queries.Response
{
    public record GetMyCartResponse(
        IReadOnlyCollection<CartItemDto> Items,
        decimal TotalPrice);
}
