using NSubstitute;
using SoldierTracker.Application.Abstractions;
using SoldierTracker.Application.Models;
using SoldierTracker.Application.Services;
using SoldierTracker.Domain.Entities;
using SoldierTracker.Domain.Interfaces;
using System.Linq.Expressions;

namespace SoldierTracker.UnitTests.Application
{
    /// <summary>
    ///     Class containing unit tests for the <see cref="SoldierService"/> class.
    /// </summary>
    [TestFixture]
    public class SoldierServiceTests
    {
        private SoldierService _sut;
        private ISignalRService _signalRServiceMock;
        private IUnitOfWork _unitOfWorkMock;
        private IRepository<Soldier> _soldierRepositoryMock;
        private Soldier _soldier;

        /// <summary>
        ///   Setup test environment for each test run.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _signalRServiceMock = Substitute.For<ISignalRService>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _soldierRepositoryMock = Substitute.For<IRepository<Soldier>>();

            _unitOfWorkMock.GetRepository<Soldier>().Returns(_soldierRepositoryMock);

            _sut = new SoldierService(_signalRServiceMock, _unitOfWorkMock);
            _soldier = new Soldier
            {
                Id = Guid.NewGuid(),
                SoldierCode = "S001",
                Rank = new Rank { Id = Guid.NewGuid(), RankName = "Private" },
                Country = new Country { Id = Guid.NewGuid(), CountryName = "PT-PT" }
            };
        }

        /// <summary>
        ///   Teardown test environment after each test run.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock.Dispose();  
        }

        /// <summary>
        ///   Given
        ///     A soldier ID for an existing soldier
        ///   When
        ///     GetSoldierByIdAsync is called with the soldier ID
        ///   Then
        ///     A SoldierDTO is returned
        /// </summary>
        [Test]
        public async Task GetSoldierById_WhenSoldierExists_ShouldReturnSoldierDTO()
        {
            // Arrange
            var soldierId = _soldier.Id;
            _soldierRepositoryMock.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Soldier, bool>>>(), Arg.Any<string>()).Returns(Task.FromResult(_soldier));

            // Act
            var result = await _sut.GetSoldierByIdAsync(soldierId);

            // Assert
            Assert.That(result, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(result.SoldierId, Is.EqualTo(soldierId));
                Assert.That(result.SoldierCode, Is.EqualTo(_soldier.SoldierCode));
                Assert.That(result.SoldierName, Is.EqualTo(_soldier.Name));
                Assert.That(result.RankName, Is.EqualTo(_soldier.Rank.RankName));
                Assert.That(result.CountryName, Is.EqualTo(_soldier.Country.CountryName));
            });
        }

        /// <summary>
        ///   Given
        ///     A soldier ID for a soldier that does not exist
        ///   When
        ///     GetSoldierByIdAsync is called with the soldier ID
        ///   Then
        ///     The result should be null.
        /// </summary>
        [Test]
        public async Task GetSoldierByIdAsync_WhenSoldierDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var soldierId = Guid.NewGuid();
            _soldierRepositoryMock.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Soldier, bool>>>(), Arg.Any<string>()).Returns(Task.FromResult<Soldier?>(null));

            // Act
            var result = await _sut.GetSoldierByIdAsync(soldierId);

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        ///   Given
        ///     A topic for soldier location updates
        ///   When
        ///     StartReceivingLocationUpdatesAsync is called with the topic name
        ///   Then
        ///     The connection should be started
        ///     And a handler should be registered for receiving SoldierData updates.
        /// </summary>
        [Test]
        public async Task StartReceivingLocationUpdatesAsync_ShouldStartConnectionAndRegisterHandler()
        {
            // Arrange
            var topicName = "SoldierLocationUpdate";
            _signalRServiceMock.StartConnectionAsync().Returns(Task.CompletedTask);

            // Act
            await _sut.StartReceivingLocationUpdatesAsync(topicName);

            // Assert
            await _signalRServiceMock.Received(1).StartConnectionAsync();
            _signalRServiceMock.Received(1).On<SoldierData>(topicName, Arg.Any<Action<SoldierData>>());
        }
    }
}
