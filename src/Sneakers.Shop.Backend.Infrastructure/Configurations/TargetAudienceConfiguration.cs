using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class TargetAudienceConfiguration : IEntityTypeConfiguration<TargetAudience>
    {
        public void Configure(EntityTypeBuilder<TargetAudience> builder)
        {
            builder.ToTable(nameof(TargetAudience));
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title).IsRequired().HasMaxLength(50);
        }
    }
}
