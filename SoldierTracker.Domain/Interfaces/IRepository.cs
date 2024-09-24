using System.Linq.Expressions;

namespace SoldierTracker.Domain.Interfaces
{
      /// <summary>
      ///   Generic repository interface for basic CRUD operations on entities of type <typeparamref name="T"/>.
      /// </summary>
      /// <typeparam name="T">The type of entity managed by the repository.</typeparam>
      public interface IRepository<T>
      {
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null, string includeProperties = null);

        Task AddAsync(T entity);

        Task Update(T entity);
      }
}
