using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        private readonly string _discountValue = "decimal(18,2)";
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.ToTable($"{nameof(Discount)}s");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.DiscountValue)
                .IsRequired()
                .HasColumnType(_discountValue);

            builder.Property(d => d.StartDate)
                .IsRequired();
            builder.Property(d => d.EndDate)
                .IsRequired();
            builder.Property(d => d.TypeDiscount)
                .IsRequired();

            builder.HasDiscriminator<string>("Discriminator")
                .HasValue<ProductDiscount>("ProductDiscount")
                .HasValue<BrandDiscount>("BrandDiscount");
        }
    }
}
