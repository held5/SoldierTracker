using SoldierTracker.UI.ViewModels;
using System.Windows;

namespace SoldierTracker.UI.Views
{
    /// <summary>
    ///     Interaction logic for SoldierMonitor.xaml
    /// </summary>
    public partial class SoldierMonitor : Window
    {
        public SoldierMonitor(SoldierMonitorViewModel soldierMonitorViewModel)
        {
            InitializeComponent();

            DataContext = soldierMonitorViewModel;
        }
    }
}