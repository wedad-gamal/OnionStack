using Core.Interfaces.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DataContext _dataContext;
        protected DbSet<T> _dbSet;
        public GenericRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _dbSet = _dataContext.Set<T>();
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task DeleteAsync(int id) => _dbSet.Remove(await _dbSet.FindAsync(id));

        public async Task<T>? GetAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();

        public void Update(T entity) => _dbSet.Update(entity);
    }
}
