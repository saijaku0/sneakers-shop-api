using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Application.Order.Command.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand(
        Guid OrderId,
        OrderStatus NewStatus,
        string? Comment) : IRequest<Result>;
}
