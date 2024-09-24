namespace SoldierTracker.Application.Services
{
    public interface ISignalRService
    {
        Task StartConnectionAsync();

        Task StopConnectionAsync();

        void On<T>(string methodName, Action<T> handler);
    }
}
