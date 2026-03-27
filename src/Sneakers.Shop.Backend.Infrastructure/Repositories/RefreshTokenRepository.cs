using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Persistence;

namespace Sneakers.Shop.Backend.Infrastructure.Repositories
{
    public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<IReadOnlyList<RefreshToken>> GetActiveTokensForUserAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .Where(rt => rt.UserId == userId &&
                    !rt.IsRevoked &&
                    rt.ExpiresAt > DateTimeOffset.UtcNow)
                .ToListAsync();
        }

        public async Task RemoveExpiredOrRevokedAsync()
        {
            var expiredOrRevoked = await _context.RefreshTokens
                .Where(rt => rt.ExpiresAt <= DateTimeOffset.UtcNow || rt.IsRevoked)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(expiredOrRevoked);
        }
    }
}
