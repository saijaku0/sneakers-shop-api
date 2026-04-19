using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Order.Command.UpdateOrderStatus
{
    public class UpdateOrderStatusHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateOrderStatusCommand, Result>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(UpdateOrderStatusCommand request, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, ct);
            if (order == null)
                return Result.Failure(Error.NotFound("Order not found."));

            order.ChangeStatus(request.NewStatus, request.Comment);
            _orderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
