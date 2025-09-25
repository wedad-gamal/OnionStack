using Core.Common;

namespace Infrastructure.Persistence.Repositories;
public class EmployeeRepository<TEntity, Tkey> : GenericRepository<TEntity, Tkey>, IEmployeeRepository<TEntity, Tkey>
    where TEntity : BaseEntity<Tkey>
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
