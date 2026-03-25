using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
    {
        public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
        {
            builder.ToTable("OrderStatusHistories");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Comment)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(o => o.NewStatus)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(o => o.OldStatus)
                .HasConversion<string>()
                .IsRequired(false);
        }
    }
}