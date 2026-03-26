using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Domain.Repositories
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UserProfile?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task AddAsync(UserProfile userProfile, CancellationToken cancellationToken = default);
        void Update(UserProfile userProfile);
    }
}
