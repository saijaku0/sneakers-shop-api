using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Persistence;

namespace Sneakers.Shop.Backend.Infrastructure.Repositories
{
    public class ProductSubmissionRepository(AppDbContext dbContext)
        : IProductSubmissionRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task AddAsync(ProductSubmission productSubmission, CancellationToken ct = default)
        {
            await _dbContext.ProductSubmissions.AddAsync(productSubmission, ct);
        }

        public void Delete(ProductSubmission productSubmission)
        {
            _dbContext.ProductSubmissions.Remove(productSubmission);
        }

        public async Task<IReadOnlyList<ProductSubmission>> GetByDropperIdAsync(
            Guid dropperId, 
            int page, 
            int pageSize, 
            CancellationToken ct = default)
        {
            var list = await _dbContext.ProductSubmissions
                .AsNoTracking()
                .Where(d => d.DropId == dropperId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return list;
        }

        public async Task<ProductSubmission?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.ProductSubmissions
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<ProductSubmission?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.ProductSubmissions
                .Include(p => p.SneakersBrand)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<IReadOnlyList<ProductSubmission>> GetByStatusAsync(
            ProductSubmissionStatus status, 
            int page, 
            int pageSize, 
            CancellationToken ct = default)
        {
            var list = await _dbContext.ProductSubmissions
                .AsNoTracking()
                .Where(p => p.Status == status)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return list;
        }

        public void Update(ProductSubmission productSubmission)
        {
            _dbContext.ProductSubmissions.Update(productSubmission);
        }
    }
}
