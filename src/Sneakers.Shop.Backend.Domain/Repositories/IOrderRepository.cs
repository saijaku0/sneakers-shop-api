using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Order>> GetByUserIdAsync(int page, int pageSize, Guid userId, CancellationToken cancellationToken = default);
        Task AddAsync(Order order, CancellationToken cancellationToken = default);
        void Update(Order order);
    }
}