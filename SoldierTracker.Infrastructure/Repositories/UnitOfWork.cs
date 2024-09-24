using Microsoft.EntityFrameworkCore;
using SoldierTracker.Domain.Interfaces;
using SoldierTracker.Infrastructure.Persistence;

namespace SoldierTracker.Infrastructure.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(SoldierTrackerDbContext dbContext)
        {
          _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public IRepository<T> GetRepository<T>()
          where T : class
          => new Repository<T>(_dbContext);

        public void Dispose()
        {
          Dispose(true);
          GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
          if (disposing)
          {
            _dbContext.Dispose();
          }
        }
    }
}
