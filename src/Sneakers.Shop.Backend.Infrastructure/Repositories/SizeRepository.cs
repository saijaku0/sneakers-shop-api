using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Sneakers.Shop.Backend.Infrastructure.Repositories
{
    public class SizeRepository(AppDbContext dbContext) : ISizeRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<IEnumerable<Size>> GetBySizesInCmAsync(IEnumerable<decimal> sizesInCm, CancellationToken cancellationToken)
        {
            return await _dbContext.Sizes
                .Where(size => sizesInCm.Contains(size.Value))
                .ToListAsync(cancellationToken);
        }
    }
}
