using System.Linq.Expressions;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class EmployeeUserTypeRepository : BaseUserUserTypeRepository<EmployeeUserType, Employee, Context>, IEmployeeUserTypeRepository
    {
        public EmployeeUserTypeRepository(IUnitOfWork<Context> unitOfWork, IUserTypeRepository userTypeRepository, IRoleRepository roleRepository) : base(unitOfWork, userTypeRepository, roleRepository) { }

        public override Expression<Func<EmployeeUserType, bool>> IdentifierFilter(Guid identifier)
        {
            return c => c.EmployeeId == identifier;
        }

        public override Expression<Func<EmployeeUserType, bool>> IdentifierContainsFilter(IEnumerable<Guid> identifiers)
        {
            return c => c.EmployeeId.HasValue && identifiers.Contains(c.EmployeeId.Value);
        }

        public override void SetIdentifier(EmployeeUserType entity, Guid identifier)
        {
            entity.EmployeeId = identifier;
        }

        public override Expression<Func<EmployeeUserType, Guid?>> GetIdentifier()
        {
            return c => c.EmployeeId;
        }

        public override Expression<Func<Employee, bool>> IsAvailableFilter(Guid? userId = null)
        {
            return c => _dbSet.Where(dut => dut.IsActive && dut.EmployeeId == c.Id && (dut.UserId == null || dut.UserId == userId)).Any();
        }

        public override Expression<Func<EmployeeUserType, Employee>> IncludeEntity()
        {
            return eut => eut.Employee!;
        }
    }
}
