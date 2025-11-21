using System.Linq.Expressions;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class DeveloperUserTypeRepository : BaseUserUserTypeRepository<DeveloperUserType, Developer, Context>, IDeveloperUserTypeRepository
    {
        public DeveloperUserTypeRepository(IUnitOfWork<Context> unitOfWork, IUserTypeRepository userTypeRepository, IRoleRepository roleRepository) : base(unitOfWork, userTypeRepository, roleRepository) { }

        public override Expression<Func<DeveloperUserType, bool>> IdentifierFilter(Guid identifier)
        {
            return c => c.DeveloperId == identifier;
        }

        public override Expression<Func<DeveloperUserType, bool>> IdentifierContainsFilter(IEnumerable<Guid> identifiers)
        {
            return c => c.DeveloperId.HasValue && identifiers.Contains(c.DeveloperId.Value);
        }

        public override void SetIdentifier(DeveloperUserType entity, Guid identifier)
        {
            entity.DeveloperId = identifier;
        }

        public override Expression<Func<DeveloperUserType, Guid?>> GetIdentifier()
        {
            return c => c.DeveloperId;
        }

        public override Expression<Func<Developer, bool>> IsAvailableFilter(Guid? userId = null)
        {
            return c => _dbSet.Where(dut => dut.IsActive && dut.DeveloperId == c.Id && (dut.UserId == null || dut.UserId == userId)).Any();
        }

        public override Expression<Func<DeveloperUserType, Developer>> IncludeEntity()
        {
            return dut => dut.Developer!;
        }
    }
}
