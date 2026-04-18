using MediatR;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Reservations.Command.CreateReservation
{
    public record CreateReservationCommand(
        Guid WarehouseItemId,
        Guid UserId,
        int Quantity) : IRequest<Result<Guid>>;
}
