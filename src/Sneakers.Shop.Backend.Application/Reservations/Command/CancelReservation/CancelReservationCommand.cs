using MediatR;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Reservations.Command.CancelReservation
{
    public record CancelReservationCommand(
        Guid ReservationId,
        Guid UserId) : IRequest<Result>;
}
