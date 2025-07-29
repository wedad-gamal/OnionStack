using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<IEnumerable<Employee>> GetALlIncludeNameAsync(string name)
        {
            var data = await _dbSet.Where(e => string.IsNullOrEmpty(name) || e.FullName.ToLower().Contains(name.ToLower())).ToListAsync();
            return data;
        }
    }
}
