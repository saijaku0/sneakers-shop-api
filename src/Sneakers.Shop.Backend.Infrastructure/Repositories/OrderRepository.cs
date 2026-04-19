using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Persistence;

namespace Sneakers.Shop.Backend.Infrastructure.Repositories
{
    public class OrderRepository(AppDbContext dbContext) 
        : IOrderRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .Include(o => o.StatusHistory)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<Order?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.WarehouseItem)
                    .ThenInclude(w => w.Product)
            .Include(o => o.StatusHistory)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Order>> GetByUserIdAsync(
            int page, 
            int pageSize, 
            Guid userId, 
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .Include(o => o.StatusHistory)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Where(o => o.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _dbContext.Orders.AddAsync(order, cancellationToken);
        }

        public void Update(Order order)
        {
            _dbContext.Orders.Update(order);
        }
    }
}