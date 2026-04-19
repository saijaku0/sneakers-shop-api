using MediatR;
using Sneakers.Shop.Backend.Application.Order.DTOs;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Order.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler(
    IOrderRepository orderRepository)
    : IRequestHandler<GetOrderByIdQuery, Result<OrderDetailsDto>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<Result<OrderDetailsDto>> Handle(
            GetOrderByIdQuery request, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(request.OrderId, ct);
            if (order == null)
                return Result<OrderDetailsDto>.Failure(Error.NotFound("Order not found."));

            if (order.UserId != request.UserId)
                return Result<OrderDetailsDto>.Failure(Error.Forbidden("Access denied."));

            var items = order.Items.Select(i => new OrderItemDto(
                WarehouseItemId: i.WarehouseItemId,
                ProductName: i.WarehouseItem.Product.ProductName,
                UnitPrice: i.UnitPrice,
                DiscountAmount: i.DiscountAmount,
                Quantity: i.Quantity,
                TotalPrice: i.TotalPrice
            )).ToList();

            var dto = new OrderDetailsDto(
                Id: order.Id,
                OrderDate: order.OrderDate,
                Status: order.Status.ToString(),
                TotalPrice: order.TotalOrderPrice,
                ShippingAddress: order.ShippingAddress,
                Items: items
            );

            return Result<OrderDetailsDto>.Success(dto);
        }
    }
}
