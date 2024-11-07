namespace SoldierTracker.Application.Abstractions
{
    public interface ISignalRService
    {
        Task StartConnectionAsync();

        Task StopConnectionAsync();

        void On<T>(string methodName, Action<T> handler);
    }
}
