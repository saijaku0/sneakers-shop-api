using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.ValueObjects;

namespace Sneakers.Shop.Backend.Application.Orders.Command.CreateOrder
{
    public record CreateOrderCommand(
        Guid UserId,
        Address ShippingAddress) : IRequest<Result<Guid>>;
}
