using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Reservations.Command.UpdateReservationQuantity
{
    public class UpdateReservationQuantityHandler(
    IReservationRepository reservationRepository,
    IGenericRepository<WarehouseItem> warehouseItemRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateReservationQuantityCommand, Result>
    {
        private readonly IReservationRepository _reservationRepository = reservationRepository;
        private readonly IGenericRepository<WarehouseItem> _warehouseItemRepository = warehouseItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(UpdateReservationQuantityCommand request, CancellationToken ct)
        {
            var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId, ct);
            if (reservation == null)
                return Result.Failure(Error.NotFound("Reservation not found."));

            if (reservation.UserId != request.UserId)
                return Result.Failure(Error.Conflict("Reservation does not belong to user."));

            var warehouseItem = await _warehouseItemRepository.GetByIdAsync(reservation.WarehouseItemId, ct);
            if (warehouseItem == null)
                return Result.Failure(Error.NotFound("Warehouse item not found."));

            var reservedByOthers = await _reservationRepository.GetReservedCountAsync(reservation.WarehouseItemId, ct) - reservation.Quantity;
            var available = warehouseItem.Quantity - reservedByOthers;

            if (request.NewQuantity > available)
                return Result.Failure(Error.Conflict($"Not enough stock. Available: {available}"));

            reservation.UpdateQuantity(request.NewQuantity);
            _reservationRepository.Update(reservation);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
