using SoldierTracker.Application.Models;

namespace SoldierTracker.Application.Services
{
    public interface ISoldierService
    {
        event EventHandler<SoldierLocationUpdateEventArgs> SoldierLocationUpdateEvent;

        Task<SoldierDTO?> GetSoldierByIdAsync(Guid soldierId);

        Task StartReceivingLocationUpdatesAsync(string topicName);
    }
}
