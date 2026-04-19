using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Order.Command.CancelOrder
{
    public class CancelOrderHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CancelOrderCommand, Result>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(CancelOrderCommand request, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(request.OrderId, ct);
            if (order == null)
                return Result.Failure(Error.NotFound("Order not found."));

            if (order.UserId != request.UserId)
                return Result.Failure(Error.Forbidden("Access denied."));

            foreach (var item in order.Items)
                item.WarehouseItem.IncreaseQuantity(item.Quantity);

            order.ChangeStatus(OrderStatus.Cancelled, "Cancelled by user");
            _orderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
