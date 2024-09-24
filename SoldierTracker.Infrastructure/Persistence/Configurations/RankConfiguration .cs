using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoldierTracker.Domain.Entities;

namespace SoldierTracker.Infrastructure.Persistence.Configurations
{
    internal class RankConfiguration : IEntityTypeConfiguration<Rank>
    {
        public void Configure(EntityTypeBuilder<Rank> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.RankName)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(r => r.RankName)
            .IsUnique();
        }
    }
}
