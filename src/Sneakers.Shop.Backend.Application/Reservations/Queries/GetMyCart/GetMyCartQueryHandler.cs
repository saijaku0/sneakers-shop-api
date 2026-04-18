using MediatR;
using Sneakers.Shop.Backend.Application.Reservations.DTOs;
using Sneakers.Shop.Backend.Application.Reservations.Queries.Response;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Reservations.Queries.GetMyCart
{
    public class GetMyCartQueryHandler(
        IReservationRepository reservationRepository)
        : IRequestHandler<GetMyCartQuery, Result<GetMyCartResponse>>
    {
        private readonly IReservationRepository _reservationRepository = reservationRepository;

        public async Task<Result<GetMyCartResponse>> Handle(
            GetMyCartQuery request,
            CancellationToken ct)
        {
            var reservations = await _reservationRepository.GetActiveByUserIdAsync(request.UserId, ct);

            var items = reservations.Select(r => new CartItemDto(
                ReservationId: r.Id,
                WarehouseItemId: r.WarehouseItemId,
                ProductId: r.WarehouseItem.ProductId,
                ProductName: r.WarehouseItem.Product.ProductName,
                Price: r.WarehouseItem.Product.BasePrice,
                Quantity: r.Quantity,
                ExpiresAt: r.ExpiresAt
            )).ToList();

            var response = new GetMyCartResponse(items, items.Sum(i => i.Price * i.Quantity));
            return Result<GetMyCartResponse>.Success(response);
        }
    }
}
