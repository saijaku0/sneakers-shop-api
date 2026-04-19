using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable($"{nameof(OrderItem)}s");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.DiscountAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.WarehouseItem)
                .WithMany()
                .HasForeignKey(x => x.WarehouseItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}