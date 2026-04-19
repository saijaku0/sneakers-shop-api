using MediatR;
using Sneakers.Shop.Backend.Application.Reservations.DTOs;
using Sneakers.Shop.Backend.Application.Reservations.Queries.Response;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Sneakers.Shop.Backend.Application.Reservations.Queries.GetMyCart
{
    public class GetMyCartQueryHandler(
        IReservationRepository reservationRepository,
        IDistributedCache cache)
        : IRequestHandler<GetMyCartQuery, Result<GetMyCartResponse>>
    {
        private readonly IReservationRepository _reservationRepository = reservationRepository;
        private readonly IDistributedCache _cache = cache;

        public async Task<Result<GetMyCartResponse>> Handle(
            GetMyCartQuery request,
            CancellationToken ct)
        {
            var cacheKey = $"cart:{request.UserId}";
            var cached = await _cache.GetStringAsync(cacheKey, ct);

            if (cached != null)
            {
                var cachedResponse = JsonSerializer.Deserialize<GetMyCartResponse>(cached);
                return Result<GetMyCartResponse>.Success(cachedResponse!);
            }

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
            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(response),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                },ct);
            return Result<GetMyCartResponse>.Success(response);
        }
    }
}
