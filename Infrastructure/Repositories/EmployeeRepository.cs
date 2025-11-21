using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Repositories;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class EmployeeRepository : BaseUserTypeRepository<Employee, EmployeeUserType, Context>, IEmployeeRepository
    {
        public EmployeeRepository(IUnitOfWork<Context> unitOfWork, IEmployeeUserTypeRepository employeeUserTypeRepository) : base(unitOfWork, employeeUserTypeRepository) { }
    }
}
