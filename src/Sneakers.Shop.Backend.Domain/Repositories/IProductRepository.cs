using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Product?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<int> GetTotalProductsCountAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<Product>> GetAllActiveAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task AddAsync(Product product, CancellationToken cancellationToken = default);
        void Update(Product product);
        void Delete(Product product);
    }
}
