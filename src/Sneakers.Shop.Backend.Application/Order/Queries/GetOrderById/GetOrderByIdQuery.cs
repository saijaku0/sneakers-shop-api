using MediatR;
using Sneakers.Shop.Backend.Application.Order.DTOs;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Order.Queries.GetOrderById
{
    public record GetOrderByIdQuery(Guid OrderId, Guid UserId)
        : IRequest<Result<OrderDetailsDto>>;
}
