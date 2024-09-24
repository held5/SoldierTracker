using System.Windows;

namespace SoldierTracker.UI.Utility
{
    public interface INotificationService
    {
        void ShowMessageBox(string message, string caption, MessageBoxButton button, MessageBoxImage icon);
    }
}
