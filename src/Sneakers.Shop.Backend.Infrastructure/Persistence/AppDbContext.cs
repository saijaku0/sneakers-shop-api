using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sneakers.Shop.Backend.Application.Common.Interfaces;
using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Infrastructure.Identity;

namespace Sneakers.Shop.Backend.Infrastructure.Persistence
{
    public class AppDbContext(
        DbContextOptions<AppDbContext> options,
        IDomainEventPublisher publisher)
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
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<SalesSnapshot> SalesSnapshots { get; set; }
        public DbSet<ProductSubmission> ProductSubmissions { get; set; }
        public DbSet<DropperPayout> DropperPayouts { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker
                .Entries<Entity>()
                .Where(e => e.Entity.Events.Any())
                .Select(e => e.Entity)
                .ToList();

            var events = entities
                .SelectMany(e => e.Events)
                .ToList();

            foreach (var entity in entities)
                entity.ClearEvents();

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in events)
                await publisher.PublishAsync(domainEvent, cancellationToken);

            return result;
        }
    }
}
