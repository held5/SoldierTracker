using Microsoft.EntityFrameworkCore;
using NSubstitute;
using SoldierTracker.Domain.Interfaces;
using SoldierTracker.Infrastructure.Repositories;

namespace SoldierTracker.UnitTests.Infrastructure
{
    /// <summary>
    ///     Class containing unit tests for the <see cref="Repository<T>"/> class.
    /// </summary>
    [TestFixture]
    public class RepositoryTests
    {
        private IRepository<TestEntity> _sut;
        private DbContext _mockDbContext;
        private DbSet<TestEntity> _mockDbSet;

        /// <summary>
        ///   Setup test environment for each test run.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _mockDbContext = Substitute.For<DbContext>();
            _mockDbSet = Substitute.For<DbSet<TestEntity>>();

            _mockDbContext.Set<TestEntity>().Returns(_mockDbSet);
            _sut = new Repository<TestEntity>(_mockDbContext);
        }

        /// <summary>
        ///   Teardown test environment after each test run.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            _mockDbContext.Dispose();
        }

        /// <summary>
        ///   Given
        ///     An entity to be added to the database
        ///   When
        ///     AddAsync is called with the entity
        ///   Then
        ///     The AddAsync method on the DbSet should be invoked once with the expected entity.
        /// </summary>
        [Test]
        public async Task AddAsync_ShouldCallAddOnDbSet()
        {
            // Arrange
            var expectedEntity = new TestEntity { Id = 1, Name = "Test" };

            // Act
            await _sut.AddAsync(expectedEntity);

            // Assert
            await _mockDbSet.Received(1).AddAsync(expectedEntity, default);
        }

        /// <summary>
        ///   Given
        ///     An entity to be updated in the database
        ///   When
        ///     Update is called with the entity
        ///   Then
        ///     The Update method on the DbSet should be invoked once with the expected entity.
        /// </summary>
        [Test]
        public async Task Update_ShouldCallUpdateOnDbSet()
        {
            // Arrange
            var expectedEntity = new TestEntity { Id = 1, Name = "Test" };

            // Act
            await _sut.Update(expectedEntity);

            // Assert
            _mockDbSet.Received(1).Update(expectedEntity);
        }

        public class TestEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
