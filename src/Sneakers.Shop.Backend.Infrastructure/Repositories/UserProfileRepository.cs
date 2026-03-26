using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Persistence;

namespace Sneakers.Shop.Backend.Infrastructure.Repositories
{
    public class UserProfileRepository(AppDbContext dbContext) : IUserProfileRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserProfiles
                .Include(u => u.WishlistItems)
                .Include(u => u.ModerationLogs)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }
        public async Task<UserProfile?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserProfiles
                .Include(u => u.WishlistItems)
                .Include(u => u.ModerationLogs)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        }
        public async Task AddAsync(UserProfile userProfile, CancellationToken cancellationToken = default)
        {
            await _dbContext.UserProfiles
                .AddAsync(userProfile, cancellationToken); 
        }
        public void Update(UserProfile userProfile)
        {
            _dbContext.UserProfiles
                .Update(userProfile);
        }
    }
}
