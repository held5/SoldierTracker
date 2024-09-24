using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoldierTracker.Domain.Entities;

namespace SoldierTracker.Infrastructure.Persistence;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<SoldierTrackerDbContextDataInit>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

internal class SoldierTrackerDbContextDataInit
{
    private readonly ILogger<SoldierTrackerDbContextDataInit> _logger;
    private readonly SoldierTrackerDbContext _context;

    public SoldierTrackerDbContextDataInit(ILogger<SoldierTrackerDbContextDataInit> logger, SoldierTrackerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!_context.Soldiers.Any())
        {
            var rank = _context.Ranks.Add(new Rank
            {
                RankName = "Sargent"
            });

            var country = _context.Countries.Add(new Country
            {
                CountryName = "PT-PT"
            });

            var soldier = _context.Soldiers.Add(new Soldier
            {
                Name = "John Doe",
                CountryID = country.Entity.Id,
                RankID = rank.Entity.Id,
                SoldierCode = "ABC",
            });

            _context.Sensors.Add(new Sensor
            {
                SensorName = "XSCJDH",
                SensorType = "AHDJ",
                SoldierID = soldier.Entity.Id, 
            });

            await _context.SaveChangesAsync();
        }

    }
}