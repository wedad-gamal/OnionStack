using Core.Entities.Common;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly Dictionary<string, object> _repositories = new();

        public UnitOfWork(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public IGenericRepository<TEntity, Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repoInstance = new Repositories.GenericRepository<TEntity, Tkey>(_context);
                _repositories.Add(type, repoInstance);
            }

            return (IGenericRepository<TEntity, Tkey>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in _context.ChangeTracker.Entries<BaseEntity<Guid>>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedOn = DateTime.UtcNow;

                if (entry.State == EntityState.Modified)
                    entry.Entity.ModifiedOn = DateTime.UtcNow;
            }

            var result = await _context.SaveChangesAsync(cancellationToken);
            await DispatchDomainEvents();
            return result;
        }

        private async Task DispatchDomainEvents()
        {
            var entitiesWithEvents = _context.ChangeTracker
                .Entries<BaseEntity<Guid>>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents != null && e.DomainEvents.Any())
                .ToList();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents!.ToList();
                entity.ClearDomainEvents();

                foreach (var domainEvent in events)
                {
                    await _mediator.Publish(domainEvent);
                }
            }
        }

        public void Dispose() => _context.Dispose();
    }
}
