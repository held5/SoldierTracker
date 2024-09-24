using Microsoft.Extensions.Options;
using NSubstitute;
using SoldierTracker.Application.Models;
using SoldierTracker.Application.Services;
using SoldierTracker.UI.Utility;
using SoldierTracker.UI.ViewModels;
using System.Windows;

namespace SoldierTracker.UnitTests.UI
{
    /// <summary>
    ///     Class containing unit tests for the <see cref="SoldierMonitorViewModel"/> class.
    /// </summary>
    public class SoldierMonitorViewModelTests
    {
        private SoldierMonitorViewModel _sut;
        private ISoldierService _soldierServiceMock;
        private INotificationService _notificationService;
        private IOptions<SensorHubSettings> _sensorHubSettings;
        private const string HubTopic = "TestTopic";

        /// <summary>
        ///   Setup test environment for each test run.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            var sensorHubSettings = new SensorHubSettings { HubTopic = HubTopic };
            _sensorHubSettings = Substitute.For<IOptions<SensorHubSettings>>();
            _sensorHubSettings.Value.Returns(sensorHubSettings);

            _notificationService = Substitute.For<INotificationService>();
            _soldierServiceMock = Substitute.For<ISoldierService>();
            _sut = new SoldierMonitorViewModel(_soldierServiceMock, _notificationService, _sensorHubSettings);

        }

        /// <summary>
        ///   Given
        ///     A command to start receiving location updates
        ///   When
        ///     StartLocationUpdatesCommand is executed
        ///   Then
        ///     StartReceivingLocationUpdatesAsync should be called once on the soldier service with the correct topic.
        /// </summary>
        [Test]
        public async Task StartLocationUpdatesCommand_ShouldCallStartReceivingLocationUpdatesAsync()
        {
            // Act
            _sut.StartLocationUpdatesCommand.Execute(null);

            // Assert
            await _soldierServiceMock.Received(1).StartReceivingLocationUpdatesAsync(HubTopic);
        }

        /// <summary>
        ///   Given
        ///     A valid soldier ID and a soldier DTO with relevant information
        ///   When
        ///     ShowSoldierInfoCommand is executed with the soldier ID
        ///   Then
        ///     GetSoldierByIdAsync should be called once on the soldier service
        ///     And a message box should display soldier info.
        /// </summary>
        [Test]
        public async Task ShowSoldierInfo_WithValidSoldierId_ShouldDisplaySoldierInfo()
        {
            // Arrange
            var soldierId = Guid.NewGuid();
            var soldierDto = new SoldierDTO(soldierId, "Test", "Test1", "ABS", "PT");

            _soldierServiceMock.GetSoldierByIdAsync(soldierId).Returns(Task.FromResult(soldierDto));

            // Act
            _sut.ShowSoldierInfoCommand.Execute(soldierId);

            // Assert
            await _soldierServiceMock.Received(1).GetSoldierByIdAsync(soldierId);

            _notificationService.Received(1).ShowMessageBox(
                Arg.Is<string>(msg => msg.Contains("Test") && msg.Contains("Test1") && msg.Contains("ABS") && msg.Contains("PT")),
                "Soldier Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        ///   Given
        ///     A soldier ID for a soldier that does not exist
        ///   When
        ///     ShowSoldierInfoCommand is executed with the soldier ID
        ///   Then
        ///     GetSoldierByIdAsync should be called once on the soldier service
        ///     And an error message box should display msg.
        /// </summary>
        [Test]
        public async Task ShowSoldierInfo_SoldierNotFound_ShouldDisplayErrorMessage()
        {
            // Arrange
            var soldierId = Guid.NewGuid();
            _soldierServiceMock.GetSoldierByIdAsync(soldierId).Returns(Task.FromResult<SoldierDTO>(default));

            // Act
            _sut.ShowSoldierInfoCommand.Execute(soldierId);

            // Assert
            await _soldierServiceMock.Received(1).GetSoldierByIdAsync(soldierId);
            _notificationService.Received(1).ShowMessageBox("Soldier not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        ///   Given
        ///     An invalid soldier ID
        ///   When
        ///     ShowSoldierInfoCommand is executed with the invalid soldier ID
        ///   Then
        ///     The soldier service should not be called to get a soldier by ID
        ///     And no message box should be displayed.
        /// </summary>
        [Test]
        public void ShowSoldierInfo_WithInvalidSoldierId_ShouldNotCallService()
        {
            // Act
            _sut.ShowSoldierInfoCommand.Execute("invalid-id");

            // Assert
            _soldierServiceMock.DidNotReceive().GetSoldierByIdAsync(Arg.Any<Guid>());
            _notificationService.DidNotReceive().ShowMessageBox(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MessageBoxButton>(), Arg.Any<MessageBoxImage>());
        }

        /// <summary>
        ///   Given
        ///     An exception occurs when starting location updates
        ///   When
        ///     StartLocationUpdatesCommand is executed
        ///   Then
        ///     An error message should be displayed indicating the connection error.
        /// </summary>
        [Test]
        public void StartLocationUpdates_WhenExceptionOccurs_ShouldShowErrorMessage()
        {
            // Arrange
            var exception = new Exception("Connection error");
            _soldierServiceMock.When(x => x.StartReceivingLocationUpdatesAsync(Arg.Any<string>())).Do(x => throw exception);

            // Act
            _sut.StartLocationUpdatesCommand.Execute(null);

            // Assert
            _notificationService.Received().ShowMessageBox(
                Arg.Any<string>(),
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
