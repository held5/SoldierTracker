using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SoldierTracker.Domain.Entities;

internal class SoldierLocationConfiguration : IEntityTypeConfiguration<SoldierLocation>
{
    public void Configure(EntityTypeBuilder<SoldierLocation> builder)
    {
        builder.HasKey(sl => sl.Id);

        builder.Property(sl => sl.LocationTimestamp)
            .IsRequired();

        builder.Property(s => s.Latitude)
            .HasColumnType("decimal(10,7)");

        builder.Property(s => s.Longitude)
            .HasColumnType("decimal(10,7)");

        builder.HasOne(sl => sl.Soldier)
            .WithMany(s => s.SoldierLocations)
            .HasForeignKey(sl => sl.SoldierId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}