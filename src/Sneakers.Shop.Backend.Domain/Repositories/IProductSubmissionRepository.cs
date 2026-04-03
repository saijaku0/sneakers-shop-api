using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Domain.Repositories
{
    public interface IProductSubmissionRepository
    {
        Task<ProductSubmission?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<ProductSubmission?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<ProductSubmission>> GetByDropperIdAsync(Guid dropperId, int page, int pageSize, CancellationToken ct = default);
        Task<IReadOnlyList<ProductSubmission>> GetByStatusAsync(ProductSubmissionStatus status, int page, int pageSize, CancellationToken ct = default);
        Task<int> GetTotalCountByDropperIdAsync(Guid dropperId, CancellationToken ct = default);
        Task AddAsync(ProductSubmission productSubmission, CancellationToken ct = default);
        void Update(ProductSubmission productSubmission);
        void Delete(ProductSubmission productSubmission);
    }
}
