using Microsoft.AspNetCore.SignalR.Client;
using SoldierTracker.Application.Services;

namespace SoldierTracker.UnitTests.Application
{
    /// <summary>
    ///     Class containing unit tests for the <see cref="HubConnectionFactory"/> class.
    /// </summary>
    [TestFixture]
    public class HubConnectionFactoryTests
    {
        private HubConnectionFactory _sut;

        /// <summary>
        ///   Setup test environment for each test run.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _sut = new HubConnectionFactory();
        }

        /// <summary>
        ///   Given
        ///     A valid hub URL for the soldier tracking system
        ///   When
        ///     CreateConnection is called with the valid URL
        ///   Then
        ///     A non-null HubConnection is returned
        ///     And the connection is in the Disconnected state.
        /// </summary>
        [Test]
        public void CreateConnection_WithValidUrl_ShouldReturnHubConnection()
        {
            // Arrange
            var hubUrl = "https://soldierTracker.com/sensorhub";

            // Act
            var connection = _sut.CreateConnection(hubUrl);

            // Assert
            Assert.That(connection, Is.Not.Null);
            Assert.That(connection.State, Is.EqualTo(HubConnectionState.Disconnected));
        }

        /// <summary>
        ///   Given
        ///     A null hub URL
        ///   When
        ///     CreateConnection is called with the null URL
        ///   Then
        ///     An ArgumentNullException is thrown
        /// </summary>
        [Test]
        public void CreateConnection_WithNullUrl_ShouldThrowArgumentNullException()
        {
            // Arrange
            string hubUrl = null;

            // Act + Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _sut.CreateConnection(hubUrl));
            Assert.That(ex.ParamName, Is.EqualTo("hubUrl"));
        }

        /// <summary>
        ///   Given
        ///     An empty hub URL
        ///   When
        ///     CreateConnection is called with the empty URL
        ///   Then
        ///     An ArgumentNullException is thrown
        /// </summary>
        [Test]
        public void CreateConnection_WithEmptyUrl_ShouldThrowArgumentNullException()
        {
            // Arrange
            var hubUrl = string.Empty;

            // Act + Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _sut.CreateConnection(hubUrl));
            Assert.That(ex.ParamName, Is.EqualTo("hubUrl"));
        }
    }
}
