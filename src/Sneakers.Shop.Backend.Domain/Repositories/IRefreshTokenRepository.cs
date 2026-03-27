using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Domain.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<IReadOnlyList<RefreshToken>> GetActiveTokensForUserAsync(Guid userId);
        Task RemoveExpiredOrRevokedAsync();
    }
}
