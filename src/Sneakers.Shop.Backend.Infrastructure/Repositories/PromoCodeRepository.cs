using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Persistence;

namespace Sneakers.Shop.Backend.Infrastructure.Repositories
{
    public class PromoCodeRepository(AppDbContext dbContext) : IPromoCodeRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<PromoCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.PromoCodes
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<PromoCode?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _dbContext.PromoCodes
                .FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
        }

        public async Task AddAsync(PromoCode promoCode, CancellationToken cancellationToken = default)
        {
            await _dbContext.PromoCodes
                .AddAsync(promoCode, cancellationToken);
        }

        public void Update(PromoCode promoCode, CancellationToken cancellationToken = default)
        {
            _dbContext.PromoCodes.Update(promoCode);
        }
    }
}
