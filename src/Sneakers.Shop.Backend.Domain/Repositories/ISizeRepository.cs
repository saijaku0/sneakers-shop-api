using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Domain.Repositories
{
    public interface ISizeRepository
    {
        Task<IEnumerable<Size>> GetBySizesInCmAsync(IEnumerable<decimal> sizesInCm, CancellationToken cancellationToken);
    }
}
