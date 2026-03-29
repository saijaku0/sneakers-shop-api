using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Infrastructure.Identity;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class DropperPayoutConfiguration : IEntityTypeConfiguration<DropperPayout>
    {
        private const string DecimalPrecision = "decimal(18,2)";

        public void Configure(EntityTypeBuilder<DropperPayout> builder)
        {
            builder.ToTable($"{nameof(DropperPayout)}s");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DropperId)
                .IsRequired();

            builder.Property(x => x.ProductId)
                .IsRequired();

            builder.Property(x => x.Amount)
                .IsRequired()
                .HasColumnType(DecimalPrecision);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.PaidAt);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.DropperId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.DropperId);
            builder.HasIndex(x => x.ProductId);
            builder.HasIndex(x => x.Status);
        }
    }
}