using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Reservations.Command.CancelReservation
{
    public class CancelReservationHandler(
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CancelReservationCommand, Result>
    {
        private readonly IReservationRepository _reservationRepository = reservationRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(CancelReservationCommand request, CancellationToken ct)
        {
            var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId, ct);
            if (reservation == null)
                return Result.Failure(Error.NotFound("Reservation not found."));

            if (reservation.UserId != request.UserId)
                return Result.Failure(Error.Conflict("Reservation does not belong to user."));

            reservation.Cancel();
            _reservationRepository.Update(reservation);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
