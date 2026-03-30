using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Domain.Repositories
{
    public interface ISalesSnapshotRepository
    {
        Task<SalesSnapshot?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<SalesSnapshot?> GetByDateAsync(DateOnly date, CancellationToken ct = default);
        Task<IReadOnlyList<SalesSnapshot>> GetRangeAsync(DateOnly from, DateOnly to, CancellationToken ct = default);
        Task<IReadOnlyList<SalesSnapshot>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
        Task AddAsync(SalesSnapshot snapshot, CancellationToken ct = default);
    }
}
