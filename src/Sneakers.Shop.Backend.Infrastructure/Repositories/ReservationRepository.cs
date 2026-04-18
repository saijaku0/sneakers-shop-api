using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Persistence;

namespace Sneakers.Shop.Backend.Infrastructure.Repositories
{
    public class ReservationRepository(AppDbContext dbContext) : IReservationRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task AddAsync(Reservation reservation, CancellationToken ct)
        {
            await _dbContext.Reservations.AddAsync(reservation, ct);
        }

        public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public async Task<int> GetReservedCountAsync(Guid warehouseItemId, CancellationToken ct)
        {
            return await _dbContext.Reservations
                .Where(r => r.WarehouseItemId == warehouseItemId
                    && r.Status == ReservationStatus.Pending)
                .SumAsync(r => r.Quantity, ct);
        }

        public async Task<IReadOnlyList<Reservation>> GetExpiredAsync(CancellationToken ct)
        {
            return await _dbContext.Reservations
                .Where(r => r.Status == ReservationStatus.Pending
                    && r.ExpiresAt < DateTimeOffset.UtcNow)
                .ToListAsync(ct);
        }

        public void Update(Reservation reservation)
        {
            _dbContext.Reservations.Update(reservation);
        }
    }
}
