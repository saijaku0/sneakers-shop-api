using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Persistence;

namespace Sneakers.Shop.Backend.Infrastructure.Repositories
{
    public class BrandRepository(AppDbContext context)
        : GenericRepository<Brand>(context), IBrandRepository
    {
    }
}
