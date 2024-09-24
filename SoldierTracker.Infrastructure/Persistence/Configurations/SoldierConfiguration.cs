using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoldierTracker.Domain.Entities;

namespace SoldierTracker.Infrastructure.Persistence.Configurations
{
    internal class SoldierConfiguration : IEntityTypeConfiguration<Soldier>
    {
        public void Configure(EntityTypeBuilder<Soldier> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.SoldierCode)
                  .IsRequired()
                  .HasMaxLength(50);

            builder.HasIndex(s => s.SoldierCode)
                .IsUnique();

            builder.Property(s => s.RankID)
              .IsRequired();

            builder.Property(s => s.CountryID)
                .IsRequired();

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(s => s.Country)
              .WithMany(c => c.Soldiers)
              .HasForeignKey(s=> s.CountryID)
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Rank)
              .WithMany(r => r.Soldiers)
              .HasForeignKey(s => s.RankID)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
