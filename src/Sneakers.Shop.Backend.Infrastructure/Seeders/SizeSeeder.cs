using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Infrastructure.Persistence;

namespace Sneakers.Shop.Backend.Infrastructure.Seeders
{
    public class SizeSeeder(AppDbContext dbContext)
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task SeedAsync()
        {
            if (await _dbContext.Sizes.AnyAsync())
                return;

            var sizes = new List<Size>
            {
                // Male CM values
                new(MeasureSizes.CM, 24.0m),
                new(MeasureSizes.CM, 25.0m),
                new(MeasureSizes.CM, 26.0m),
                new(MeasureSizes.CM, 27.0m),
                new(MeasureSizes.CM, 28.0m),
                new(MeasureSizes.CM, 29.0m),
                new(MeasureSizes.CM, 30.0m),

                // Female only CM values
                new(MeasureSizes.CM, 22.0m),
                new(MeasureSizes.CM, 23.0m),
            };

            await _dbContext.Sizes.AddRangeAsync(sizes);
            await _dbContext.SaveChangesAsync();
        }
    }
}