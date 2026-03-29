using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class SalesSnapshotConfiguration : IEntityTypeConfiguration<SalesSnapshot>
    {
        private const string DecimalPrecision = "decimal(18,2)";

        public void Configure(EntityTypeBuilder<SalesSnapshot> builder)
        {
            builder.ToTable($"{nameof(SalesSnapshot)}s");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Date)
                .IsRequired();

            builder.Property(x => x.TotalRevenue)
                .IsRequired()
                .HasColumnType(DecimalPrecision);

            builder.Property(x => x.TotalOrders)
                .IsRequired();

            builder.Property(x => x.TotalPayouts)
                .IsRequired()
                .HasColumnType(DecimalPrecision);

            builder.Property(x => x.AverageOrderValue)
                .IsRequired()
                .HasColumnType(DecimalPrecision);

            builder.HasIndex(x => x.Date)
                .IsUnique();
        }
    }
}