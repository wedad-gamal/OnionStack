using Core.Entities.Common;

namespace Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default) => await _dbSet.FindAsync(id, cancellationToken);
        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) => await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) => await _dbSet.AddAsync(entity, cancellationToken);
        public void Update(TEntity entity) => _dbSet.Update(entity);
        public void Delete(TEntity entity) => _dbSet.Remove(entity);


    }
}
