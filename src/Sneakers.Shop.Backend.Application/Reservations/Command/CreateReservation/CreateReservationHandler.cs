using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Reservations.Command.CreateReservation
{
    public class CreateReservationHandler(
        IReservationRepository reservationRepository,
        IGenericRepository<WarehouseItem> warehouseItemRepository,
        IUnitOfWork unitOfWork) 
        : IRequestHandler<CreateReservationCommand, Result<Guid>>
    {
        private readonly IReservationRepository _reservationRepository = reservationRepository;
        private readonly IGenericRepository<WarehouseItem> _warehouseItemRepository = warehouseItemRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result<Guid>> Handle(CreateReservationCommand request, CancellationToken ct)
        {
            var warehouseItem = await _warehouseItemRepository.GetByIdAsync(request.WarehouseItemId, ct);
            if (warehouseItem == null) 
                return Result<Guid>.Failure(Error.NotFound("Warehouse item not found."));

            var reservedCount = await _reservationRepository.GetReservedCountAsync(request.WarehouseItemId, ct);
            var available = warehouseItem.Quantity - reservedCount;

            if (request.Quantity > available)
                return Result<Guid>.Failure(Error.Conflict($"Not enough stock. Available: {available}"));

            var reservation = new Reservation
            (
                request.WarehouseItemId,
                request.UserId,
                DateTimeOffset.UtcNow.AddMinutes(120),
                request.Quantity
            );
            await _reservationRepository.AddAsync(reservation, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<Guid>.Success(reservation.Id);
        }
    }
}
