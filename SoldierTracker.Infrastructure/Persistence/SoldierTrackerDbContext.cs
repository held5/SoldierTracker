using Microsoft.EntityFrameworkCore;
using SoldierTracker.Domain.Entities;
using System.Reflection;

namespace SoldierTracker.Infrastructure.Persistence
{
    public class SoldierTrackerDbContext : DbContext
    {
        public SoldierTrackerDbContext(DbContextOptions<SoldierTrackerDbContext> options)
          : base(options)
        {
        }

        public DbSet<Soldier> Soldiers { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<Rank> Ranks { get; set; }

        public DbSet<SoldierLocation> SoldierLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
