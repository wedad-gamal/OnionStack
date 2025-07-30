using Infrastructure.Context;
using System.Collections.Concurrent;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        private readonly Func<Type, object> _serviceFactory;
        private readonly ConcurrentDictionary<Type, object> _resolved = new();
        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            return (TRepository)_resolved.GetOrAdd(typeof(TRepository), t => _serviceFactory(t));
        }

        //to close connection between UnitOfWork and Database
        public void Dispose() => _dataContext.Dispose();

        public int SaveChanges() => _dataContext.SaveChanges();
    }
}
