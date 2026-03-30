using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Domain.Repositories
{
    public interface IDiscountRepository
    {
        Task<Discount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Discount>> GetActiveAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Discount discount, CancellationToken cancellationToken = default);
        void Update(Discount discount, CancellationToken cancellationToken = default);
        void Delete(Discount discount, CancellationToken cancellationToken = default);
    }
}
