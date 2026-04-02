using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sneakers.Shop.Backend.Application.Auth.Command;
using Sneakers.Shop.Backend.Application.Auth.Interfaces;
using Sneakers.Shop.Backend.Application.Interfaces;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Infrastructure.Auth;
using Sneakers.Shop.Backend.Infrastructure.Identity;
using Sneakers.Shop.Backend.Infrastructure.Persistence;
using Sneakers.Shop.Backend.Infrastructure.Repositories;
using System.Text;

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
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IProductRepository, ProductRepository>();
            service.AddScoped<IOrderRepository, OrderRepository>();
            service.AddScoped<IUserProfileRepository, UserProfileRepository>();
            service.AddScoped<IDropperPayoutRepository, DropperPayoutRepository>();
            service.AddScoped<ISalesSnapshotRepository, SalesSnapshotRepository>();
            service.AddScoped<IProductSubmissionRepository, ProductSubmissionRepository>();
            service.AddScoped<IBrandRepository, BrandRepository>();

            service.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            service.AddScoped<IJwtService, JwtService>();
            service.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            service.AddScoped<IIdentityService, IdentityService>();
            service.AddScoped<IAuthService, AuthService>();

            service.AddScoped<RoleSeeder>();
            service.AddAuthorization();

            service.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!)),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return service;
        }
    }
}
