using Core.Common;

namespace Core.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity, Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
