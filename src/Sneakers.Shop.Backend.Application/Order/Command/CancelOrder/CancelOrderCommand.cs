using MediatR;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Order.Command.CancelOrder
{
    public record CancelOrderCommand(
        Guid OrderId,
        Guid UserId) : IRequest<Result>;
}
