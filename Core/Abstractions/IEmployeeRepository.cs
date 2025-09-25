using Core.Common;

namespace Core.Abstractions
{
    public interface IEmployeeRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey>
        where TEntity : BaseEntity<Tkey>
    {

    }
}
