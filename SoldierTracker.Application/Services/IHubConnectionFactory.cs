using Microsoft.AspNetCore.SignalR.Client;

internal interface IHubConnectionFactory
{
    HubConnection CreateConnection(string hubUrl);
}