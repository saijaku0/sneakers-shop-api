using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Domain.Repositories
{
    public interface IPromoCodeRepository
    {
        Task<PromoCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PromoCode?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task AddAsync(PromoCode promoCode, CancellationToken cancellationToken = default);
        void Update(PromoCode promoCode, CancellationToken cancellationToken = default);
    }
}
