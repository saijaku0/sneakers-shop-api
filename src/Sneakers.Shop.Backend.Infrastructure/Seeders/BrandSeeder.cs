using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Infrastructure.Seeders
{
    public class BrandSeeder(
        IBrandRepository brandRepository,
        IUnitOfWork unitOfWork)
    {
        private readonly IBrandRepository _brandRepository = brandRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private static readonly Guid TestCategoryId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        public async Task SeedAsync()
        {
            if (await _brandRepository.GetAllAsync() is not { Count: 0 })
                return;
            var brand = new Brand(TestCategoryId, "Nike");
            await _brandRepository.AddAsync(brand);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
