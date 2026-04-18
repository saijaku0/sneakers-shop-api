using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Domain.Repositories
{
    public interface IReservationRepository
    {
        Task AddAsync(Reservation reservation, CancellationToken ct);
        Task<Reservation?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Reservation>> GetActiveByUserIdAsync(Guid userId, CancellationToken ct);
        Task<int> GetReservedCountAsync(Guid warehouseItemId, CancellationToken ct);
        Task<IReadOnlyList<Reservation>> GetExpiredAsync(CancellationToken ct);
        void Update(Reservation reservation);
    }
}
