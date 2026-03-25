using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class PromoCodeConfiguration : IEntityTypeConfiguration<PromoCode>
    {
        public void Configure(EntityTypeBuilder<PromoCode> builder)
        {
            builder.ToTable($"{nameof(PromoCode)}s");
            builder.HasKey(d => d.Id);

            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(p => p.DiscountValue)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(p => p.DiscountType)
                .IsRequired();
            builder.Property(p => p.ValidFrom)
                .IsRequired();
            builder.Property(p => p.ExpirationDate)
                .IsRequired();
            builder.Property(p => p.IsUsed)
                .IsRequired();
            builder.HasIndex(p => p.Code)
                .IsUnique();

            builder.HasOne<UserProfile>()
                .WithMany()                        
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Product>()
                .WithMany()                         
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
