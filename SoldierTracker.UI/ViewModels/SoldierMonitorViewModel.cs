using Microsoft.Extensions.Options;
using SoldierTracker.Application.Models;
using SoldierTracker.Application.Services;
using SoldierTracker.Infrastructure;
using SoldierTracker.UI.Models;
using SoldierTracker.UI.Utility;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SoldierTracker.UI.ViewModels
{
    public class SoldierMonitorViewModel : BaseViewModel
    {
        private readonly ISoldierService _soldierService;
        private readonly INotificationService _notificationService;
        private readonly IOptions<SensorHubSettings> _sensorHubSettings;
        private readonly Dictionary<Guid, SoldierInfo> _soldiersById = new();

        public ObservableCollection<SoldierInfo> Soldiers { get; set; } = new();

        public ICommand StartLocationUpdatesCommand { get; }

        public ICommand ShowSoldierInfoCommand { get; }

        public SoldierMonitorViewModel(
            ISoldierService soldierService,
            INotificationService notificationService,
            IOptions<SensorHubSettings> sensorHubSettings)
        {
            _soldierService = soldierService;
            _sensorHubSettings = sensorHubSettings;
            _notificationService = notificationService;
            StartLocationUpdatesCommand = new RelayCommand(StartLocationUpdates);
            ShowSoldierInfoCommand = new RelayCommand(ShowSoldierInfo);

            _soldierService.SoldierLocationUpdateEvent += OnSoldierLocationUpdateEvent;
        }

        private async void ShowSoldierInfo(object soldierId)
        {
            if (soldierId is not Guid)
            {
                return;
            }

            try
            {
                var selectedSoldier = await _soldierService.GetSoldierByIdAsync((Guid)soldierId);

                if (selectedSoldier != null)
                {
                    _notificationService.ShowMessageBox(
                        $"Soldier Id: {selectedSoldier.SoldierId}\n" +
                        $"Name: {selectedSoldier.SoldierName}\n" +
                        $"Rank: {selectedSoldier.RankName}\n" +
                        $"Country: {selectedSoldier.CountryName}",
                        "Soldier Information",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    _notificationService.ShowMessageBox("Soldier not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                _notificationService.ShowMessageBox($"An error occurred while retrieving soldier information:\n{ex.Message}",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private async void StartLocationUpdates(object obj)
        {
            try
            {
                await _soldierService.StartReceivingLocationUpdatesAsync(_sensorHubSettings.Value.HubTopic);
            }
            catch (Exception ex)
            {
                _notificationService.ShowMessageBox($"An error occurred while coonecting for live updates:\n{ex.Message}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            }

        }

        private void OnSoldierLocationUpdateEvent(object? sender, SoldierLocationUpdateEventArgs e)
        {
            if (_soldiersById.TryGetValue(e.SoldierId, out SoldierInfo? soldierInfo))
            {
                App.Current.Dispatcher.Invoke(() => soldierInfo.Locations.Add(new Location(e.Latitude, e.Longitude)));

            }
            else
            {
                var newSoldierInfo = new SoldierInfo
                {
                    SoldierId = e.SoldierId,
                    Locations = new ObservableCollection<Location>
                    {
                        new Location(e.Latitude, e.Longitude)
                    }
                };

                _soldiersById.Add(e.SoldierId, newSoldierInfo);

                App.Current.Dispatcher.Invoke(() => Soldiers.Add(newSoldierInfo));
            }
        }
    }
}
