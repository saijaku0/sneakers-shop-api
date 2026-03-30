using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Domain.Repositories
{
    public interface IDropperPayoutRepository
    {
        Task<DropperPayout?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<DropperPayout>> GetByDropperIdAsync(Guid dropperId, int page, int pageSize, CancellationToken ct = default);
        Task<IReadOnlyList<DropperPayout>> GetDropperPayoutsAsync(PayoutStatus status, int page, int pageSize, CancellationToken ct = default);
        Task AddAsync(DropperPayout payout, CancellationToken ct = default);
        void Update(DropperPayout dropperPayout);
        void Delete(DropperPayout dropperPayout);
    }
}
