using Microsoft.AspNetCore.SignalR;
using SensorHub.API.Models;

namespace SensorHub.API
{
    internal class MessageHub : Hub
    {
        public async Task SendSoldierUpdate(Soldier soldier)
        {
            await Clients.All.SendAsync("SoldierLocationUpdate", soldier);
        }
    }
}
