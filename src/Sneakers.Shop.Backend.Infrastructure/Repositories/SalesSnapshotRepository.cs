using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Persistence;

namespace Sneakers.Shop.Backend.Infrastructure.Repositories
{
    public class SalesSnapshotRepository(AppDbContext dbContext) 
         : ISalesSnapshotRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task AddAsync(SalesSnapshot snapshot, CancellationToken ct = default)
        {
            await _dbContext.SalesSnapshots.AddAsync(snapshot, ct);
        }

        public async Task<IReadOnlyList<SalesSnapshot>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
        {
            return await _dbContext.SalesSnapshots
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<SalesSnapshot?> GetByDateAsync(DateOnly date, CancellationToken ct = default)
        {
            return await _dbContext.SalesSnapshots
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Date == date, ct);
        }

        public async Task<SalesSnapshot?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.SalesSnapshots
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        public async Task<IReadOnlyList<SalesSnapshot>> GetRangeAsync(DateOnly from, DateOnly to, CancellationToken ct = default)
        {
            return await _dbContext.SalesSnapshots
                .AsNoTracking()
                .Where(s => s.Date >= from && s.Date <= to)
                .ToListAsync(ct);
        }
    }
}
