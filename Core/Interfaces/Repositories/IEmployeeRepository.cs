using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetALlIncludeNameAsync(string name);
    }
}
