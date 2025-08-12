using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DataContext dataContext) : base(dataContext)
        {
        }


    }
}
