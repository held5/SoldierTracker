using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SoldierTracker.Infrastructure.Persistence;
using System.IO;

namespace SoldierTracker.UI
{
    /// <summary>
    ///     Creates SoldierTrackerContext for design-time operations like migrations.
    ///     Reads the connection string from appsettings. Not used at runtime.
    /// </summary>
    public class DbContextFactory : IDesignTimeDbContextFactory<SoldierTrackerDbContext>
    {
        public SoldierTrackerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SoldierTrackerDbContext>();

           var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            return new SoldierTrackerDbContext(optionsBuilder.Options);
        }
    }
}
