namespace SoldierTracker.Domain.Interfaces
{
  /// <summary>
  ///   Represents a unit of work interface for managing transactions and repositories.
  /// </summary>
  public interface IUnitOfWork : IDisposable
  {
    /// <summary>
    ///   Asynchronously saves all changes made in this unit of work to the underlying database.
    /// </summary>
    /// <returns>The task result with the number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    ///   Gets a repository instance for the specified entity type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of entity for which the repository is requested.</typeparam>
    /// <returns>An instance of <see cref="IRepository{T}"/>.</returns>
    IRepository<T> GetRepository<T>()
      where T : class;
  }
}
