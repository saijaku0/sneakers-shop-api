using MediatR;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Reservations.Command.UpdateReservationQuantity
{
    public record UpdateReservationQuantityCommand(
        Guid ReservationId,
        Guid UserId,
        int NewQuantity) : IRequest<Result>;
}
