using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
    {
        public void Configure(EntityTypeBuilder<WishlistItem> builder)
        {
            builder.ToTable($"{nameof(WishlistItem)}s");
            builder.HasKey(w => w.Id);

            builder.HasIndex(w => new { w.UserId, w.ProductId })
                .IsUnique();

            builder.HasOne(w => w.Product)
               .WithMany()
               .HasForeignKey(w => w.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}