using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using SoldierTracker.Application.Abstractions;
using SoldierTracker.Application.Models;

namespace SoldierTracker.Infrastructure.Services
{
    internal class SignalRService : ISignalRService, IDisposable
    {
        private HubConnection _connection;
        private bool _disposed;

        public SignalRService(IHubConnectionFactory hubConnectionFactory, IOptions<SensorHubSettings> sensorHubSettings)
        {
            _connection = hubConnectionFactory.CreateConnection(sensorHubSettings.Value.HubUrl);
        }

        public async Task StartConnectionAsync()
        {
            if (!IsConnected)
            {
                SetupConnectionHandlers();

                await _connection.StartAsync();
                Console.WriteLine("SignalR connection started.");
            }
        }

        public async Task StopConnectionAsync()
        {
            if (IsConnected)
            {
                await _connection.StopAsync();
                Console.WriteLine("SignalR connection stopped.");
            }
        }

        public void On<T>(string methodName, Action<T> handler)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException(nameof(methodName), "Method name cannot be null or empty.");
            }

            if (!IsConnected)
            {
                throw new InvalidOperationException("Hub not connected.");
            }

            _connection.On(methodName, handler);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _connection.DisposeAsync().AsTask().Wait();
            _disposed = true;

            GC.SuppressFinalize(this);
        }

        private void SetupConnectionHandlers()
        {
            _connection.Closed += async (error) =>
            {
                Console.WriteLine("Connection closed. Reconnecting...");
                await Task.Delay(5000);
                try
                {
                    await _connection.StartAsync();
                    Console.WriteLine("Reconnected.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to reconnect: {ex.Message}");
                }
            };
        }

        private bool IsConnected => _connection.State == HubConnectionState.Connected;
    }
}
