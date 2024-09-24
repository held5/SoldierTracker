using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoldierTracker.Domain.Entities;

namespace SoldierTracker.Infrastructure.Persistence.Configurations
{
    internal class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CountryName)
              .IsRequired()
              .HasMaxLength(10);

            builder.HasIndex(c => c.CountryName)
                .IsUnique();
        }
    }
}
