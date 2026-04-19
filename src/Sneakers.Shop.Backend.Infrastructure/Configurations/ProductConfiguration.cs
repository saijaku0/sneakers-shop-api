using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        private readonly string _priceType = "decimal(18,2)";
        private readonly string _warehouseItems = "_warehouseItems";
        private readonly string _comments = "_comments";

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable($"{nameof(Product)}s");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(p => p.Model)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(d => d.Description)
                .IsRequired()
                .HasMaxLength(2000);
            builder.Property(b => b.BasePrice)
                .IsRequired()
                .HasColumnType(_priceType);

            builder.HasOne(p => p.SneakersBrand)
                .WithMany()
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(w => w.WarehouseItems)
                .WithOne(w => w.Product)
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(p => p.WarehouseItems)
                .HasField(_warehouseItems)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(w => w.Comments)
                .WithOne()
                .HasForeignKey(w => w.ProductId);
            builder.Navigation(p => p.Comments)
                .HasField(_comments)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(p => p.ImagesUrls);
        }
    }
}
