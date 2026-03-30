using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Persistence;

namespace Sneakers.Shop.Backend.Infrastructure.Repositories
{
    public class ProductRepository(AppDbContext dbContext) 
        : IProductRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Product?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .Include(p => p.WarehouseItems)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetAllActiveAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Where(p => p.IsActive)
                .Skip((page-1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _dbContext.Products
                .AddAsync(product, cancellationToken);
        }

        public void Update(Product product)
        {
            _dbContext.Products
                .Update(product);
        }

        public void Delete(Product product)
        {
            _dbContext.Products
                .Remove(product);
        }
    }
}
