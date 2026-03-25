using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class WarehouseItemConfiguration : IEntityTypeConfiguration<WarehouseItem>
    {
        public void Configure(EntityTypeBuilder<WarehouseItem> builder)
        {
            builder.ToTable($"{nameof(WarehouseItem)}s");
            builder.HasKey(x => x.Id);

            builder.Property(w => w.Quantity)
                .IsRequired();

            builder.HasOne<Size>()
                .WithMany()
                .HasForeignKey(s => s.SizeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new {x.ProductId, x.SizeId})
                .IsUnique();
        }
    }
}
