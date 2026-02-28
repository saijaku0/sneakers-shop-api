using Microsoft.Extensions.DependencyInjection;

namespace Sneakers.Shop.Backend.Application.Injection
{
    public static class MediatRInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(MediatRInjection).Assembly));
            return services;
        }
    }
}
