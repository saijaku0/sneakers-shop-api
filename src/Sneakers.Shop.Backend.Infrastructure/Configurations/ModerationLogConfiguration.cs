using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Infrastructure.Configurations
{
    public class ModerationLogConfiguration : IEntityTypeConfiguration<ModerationLog>
    {
        public void Configure(EntityTypeBuilder<ModerationLog> builder)
        {
            builder.ToTable($"{nameof(ModerationLog)}s");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Reason)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.Action)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}