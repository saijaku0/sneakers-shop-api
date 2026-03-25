using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        private readonly string _wishlistItems = "_wishlistItems";
        private readonly string _moderationLogs = "_moderationLogs";

        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable($"{nameof(UserProfile)}s");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.OwnsOne(u => u.DefaultShippingAddress, address =>
            {
                address.Property(a => a.Country).IsRequired().HasMaxLength(100);
                address.Property(a => a.State).IsRequired().HasMaxLength(100);
                address.Property(a => a.City).IsRequired().HasMaxLength(100);
                address.Property(a => a.Street).IsRequired().HasMaxLength(100);
                address.Property(a => a.HouseNumber).IsRequired().HasMaxLength(20);
                address.Property(a => a.PostalCode).IsRequired();
            });


            builder.HasMany(u => u.WishlistItems)
                .WithOne()
                .HasForeignKey(wi => wi.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(u => u.WishlistItems)
                .HasField(_wishlistItems)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(u => u.ModerationLogs)
                .WithOne()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(u => u.ModerationLogs)
                .HasField(_moderationLogs)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}