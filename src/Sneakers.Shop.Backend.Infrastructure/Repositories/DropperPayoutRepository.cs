using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Persistence;

namespace Sneakers.Shop.Backend.Infrastructure.Repositories
{
    public class DropperPayoutRepository(AppDbContext dbContext) 
         : IDropperPayoutRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task AddAsync(DropperPayout payout, CancellationToken ct = default)
        {
            await _dbContext.DropperPayouts.AddAsync(payout, ct);
        }

        public void Delete(DropperPayout dropperPayout)
        {
            _dbContext.DropperPayouts.Remove(dropperPayout);
        }

        public async Task<DropperPayout?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.DropperPayouts
                .FirstOrDefaultAsync(d => d.Id == id, ct);
        }

        public async Task<IReadOnlyList<DropperPayout>> GetByDropperIdAsync(
            Guid dropperId, 
            int page, 
            int pageSize, 
            CancellationToken ct = default)
        {
            return await _dbContext.DropperPayouts
                .AsNoTracking()
                .Where(p => p.DropperId == dropperId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<DropperPayout>> GetDropperPayoutsAsync(
            PayoutStatus status, 
            int page, 
            int pageSize, 
            CancellationToken ct = default)
        {
            return await _dbContext.DropperPayouts
                .AsNoTracking()
                .Where(p => p.Status == status)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public void Update(DropperPayout dropperPayout)
        {
            _dbContext.DropperPayouts.Update(dropperPayout);
        }
    }
}
