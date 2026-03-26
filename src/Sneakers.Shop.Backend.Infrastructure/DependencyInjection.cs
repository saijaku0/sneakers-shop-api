using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Persistence;
using Sneakers.Shop.Backend.Infrastructure.Repositories;

namespace Sneakers.Shop.Backend.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection service, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            service.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddScoped<IPromoCodeRepository, PromoCodeRepository>();
            service.AddScoped<IDiscountRepository, DiscountRepository>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IProductRepository, ProductRepository>();
            service.AddScoped<IOrderRepository, OrderRepository>();
            service.AddScoped<IUserProfileRepository, UserProfileRepository>();

            return service;
        }
    }
}
