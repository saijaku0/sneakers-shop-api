using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable($"{nameof(RefreshToken)}s");
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(256);
            builder.HasIndex(rt => rt.Token)
                .IsUnique();

            builder.HasOne<UserProfile>()
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(rt => rt.CreatedAt)
                .IsRequired();
            builder.Property(rt => rt.ExpiresAt)
                .IsRequired();
            builder.Property(rt => rt.IsRevoked)
                .IsRequired();
        }
    }
}
