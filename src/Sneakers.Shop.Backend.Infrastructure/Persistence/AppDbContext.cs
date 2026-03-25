using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Infrastructure.Identity;

namespace Sneakers.Shop.Backend.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options)
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
    {
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ModerationLog> ModerationLogs { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Brand> Brands { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
