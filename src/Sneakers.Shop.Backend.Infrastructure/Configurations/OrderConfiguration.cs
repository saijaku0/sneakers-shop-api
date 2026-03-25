using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        private readonly string _items = "_items";
        private readonly string _statusHistory = "_statusHistory";

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable($"{nameof(Order)}s");
            builder.HasKey(o => o.Id);

            builder.OwnsOne(o => o.ShippingAddress, address =>
            {
                address.Property(a => a.Country).IsRequired().HasMaxLength(100);
                address.Property(a => a.State).IsRequired().HasMaxLength(100);
                address.Property(a => a.City).IsRequired().HasMaxLength(100);
                address.Property(a => a.Street).IsRequired().HasMaxLength(100);
                address.Property(a => a.HouseNumber).IsRequired().HasMaxLength(20);
                address.Property(a => a.PostalCode).IsRequired();
            });

            builder.HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(o => o.Items)
                .HasField(_items)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(o => o.StatusHistory)
                .WithOne()
                .HasForeignKey(osh => osh.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(o => o.StatusHistory)
                .HasField(_statusHistory)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(o => o.Status)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}