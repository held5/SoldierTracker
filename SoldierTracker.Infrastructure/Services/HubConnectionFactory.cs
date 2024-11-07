using Microsoft.AspNetCore.SignalR.Client;

namespace SoldierTracker.Infrastructure.Services
{
    internal class HubConnectionFactory : IHubConnectionFactory
    {
        public HubConnection CreateConnection(string hubUrl)
        {
            if (string.IsNullOrEmpty(hubUrl))
            {
                throw new ArgumentNullException(nameof(hubUrl), "Hub URL cannot be null or empty.");
            }

            return new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();
        }
    }
}
