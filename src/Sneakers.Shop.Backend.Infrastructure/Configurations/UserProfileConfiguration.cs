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

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Lastname)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            builder.OwnsOne(u => u.DefaultShippingAddress, address =>
            {
                address.Property(a => a.Country).HasMaxLength(100);
                address.Property(a => a.State).HasMaxLength(100);
                address.Property(a => a.City).HasMaxLength(100);
                address.Property(a => a.Street).HasMaxLength(100);
                address.Property(a => a.HouseNumber).HasMaxLength(20);
                address.Property(a => a.PostalCode).HasMaxLength(20);
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