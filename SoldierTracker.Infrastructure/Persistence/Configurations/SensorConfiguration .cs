using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoldierTracker.Domain.Entities;

namespace SoldierTracker.Infrastructure.Persistence.Configurations
{
    internal class SensorConfiguration : IEntityTypeConfiguration<Sensor>
    {
        public void Configure(EntityTypeBuilder<Sensor> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.SensorName)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasIndex(s => s.SensorName)
                .IsUnique();

            builder.Property(s => s.SensorType)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(s => s.Soldier)
                .WithMany(sol => sol.Sensors)
                .HasForeignKey(s => s.SoldierID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
