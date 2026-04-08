using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Infrastructure.Identity;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class ProductSubmissionConfiguration : IEntityTypeConfiguration<ProductSubmission>
    {
        private const string DecimalPrecision = "decimal(18,2)";

        public void Configure(EntityTypeBuilder<ProductSubmission> builder)
        {
            builder.ToTable($"{nameof(ProductSubmission)}s");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DropId)
                .IsRequired();

            builder.Property(x => x.BrandId)
                .IsRequired();

            builder.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.OwnsMany(x => x.SubmissionSizes, sb =>
            {
                sb.Property(x => x.Quantity)
                    .IsRequired();
                sb.Property(x => x.SizeInCm)
                    .IsRequired()
                    .HasColumnType(DecimalPrecision);
            });

            builder.Property(x => x.Model)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.BasePrice)
                .IsRequired()
                .HasColumnType(DecimalPrecision);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.RejectionReason)
                .HasMaxLength(500);

            builder.Property(x => x.ModeratorId);

            builder.Property(x => x.CheckedAt);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.DropId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.ModeratorId)
                .IsRequired(false)  
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SneakersBrand)
                .WithMany()
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.TargetAudience)
                .IsRequired()
                .HasConversion<string>();

            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.DropId);
        }
    }
}
