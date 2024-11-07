using Microsoft.AspNetCore.SignalR;
using SensorHub.API.Models;

namespace SensorHub.API
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<MessageHub> _messageHub;
        private readonly PeriodicTimer _periodicTimer;
        // For simulation purposes
        private const string SoldierCode = "ABC";
        private const string SensorName = "XSCJDH";
        private int _workerCount = 0;

        public Worker(ILogger<Worker> logger, IHubContext<MessageHub> messageHub)
        {
            _logger = logger;
            _messageHub = messageHub;
            _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _periodicTimer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    var soldier = GenerateSoldierCoordinates();
                    await _messageHub.Clients.All.SendAsync("SoldierLocationUpdate", soldier);

                    _workerCount++;
                    _logger.LogInformation("Data number sent: {DataCount}", _workerCount);

                    if (_workerCount == 1500)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(10));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending soldier data");
                }
            }
        }

        private Soldier GenerateSoldierCoordinates()
        {
            // Lisbon by default
            var baseLatitude = 38.7223m;
            var baseLongitude = -9.1393m;
            var timestamp = DateTimeOffset.UtcNow;

            // Random offset in degrees
            decimal latitudeOffset = (decimal)(new Random().NextDouble() * 0.01 - 0.005);
            decimal longitudeOffset = (decimal)(new Random().NextDouble() * 0.01 - 0.005);

            var randomLatitude = baseLatitude + latitudeOffset;
            var randomLongitude = baseLongitude + longitudeOffset;

            return new Soldier(SoldierCode, SensorName, randomLatitude, randomLongitude, timestamp);
        }
    }
}