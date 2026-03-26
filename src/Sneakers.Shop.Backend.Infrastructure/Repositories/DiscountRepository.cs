using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Persistence;

namespace Sneakers.Shop.Backend.Infrastructure.Repositories
{
    public class DiscountRepository(AppDbContext dbContext) : IDiscountRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<Discount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Discounts
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Discount>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            return await _dbContext.Discounts
                .AsNoTracking() 
                .Where(d => d.StartDate <= now && d.EndDate >= now)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Discount discount, CancellationToken cancellationToken = default)
        {
            await _dbContext.Discounts.AddAsync(discount, cancellationToken);
        }

        public void Update(Discount discount, CancellationToken cancellationToken = default)
        {
            _dbContext.Discounts.Update(discount);
        }

        public void Delete(Discount discount, CancellationToken cancellationToken = default)
        {
            _dbContext.Discounts.Remove(discount);
        }
    }
}
