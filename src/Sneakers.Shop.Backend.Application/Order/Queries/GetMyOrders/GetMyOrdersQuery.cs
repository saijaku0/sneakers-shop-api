using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Application.Order.DTOs;

namespace Sneakers.Shop.Backend.Application.Order.Queries.GetMyOrders
{
    public record GetMyOrdersQuery(
        Guid UserId,
        int Page = 1,
        int PageSize = 10) 
        : IRequest<Result<IReadOnlyList<OrderDto>>>;
}
