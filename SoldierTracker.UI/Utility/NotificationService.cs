using System.Windows;

namespace SoldierTracker.UI.Utility
{
    public class NotificationService : INotificationService
    {
        public void ShowMessageBox(string message, string caption, MessageBoxButton button, MessageBoxImage icon)
            => MessageBox.Show(message, caption, button, icon);
    }
}
