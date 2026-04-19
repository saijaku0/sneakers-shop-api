using MediatR;
using Sneakers.Shop.Backend.Application.Order.DTOs;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Order.Queries.GetMyOrders
{
    public class GetMyOrdersQueryHandler(
    IOrderRepository orderRepository)
    : IRequestHandler<GetMyOrdersQuery, Result<IReadOnlyList<OrderDto>>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<Result<IReadOnlyList<OrderDto>>> Handle(
            GetMyOrdersQuery request, 
            CancellationToken ct)
        {
            var orders = await _orderRepository.GetByUserIdAsync(
                request.Page, 
                request.PageSize, 
                request.UserId, 
                ct);

            var dtos = orders.Select(o => new OrderDto(
                Id: o.Id,
                OrderDate: o.OrderDate,
                Status: o.Status.ToString(),
                TotalPrice: o.TotalOrderPrice,
                ItemsCount: o.Items.Count
            )).ToList();

            return Result<IReadOnlyList<OrderDto>>.Success(dtos);
        }
    }
}
