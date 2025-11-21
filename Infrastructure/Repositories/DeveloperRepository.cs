using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class DeveloperRepository : BaseUserTypeRepository<Developer, DeveloperUserType, Context>, IDeveloperRepository
    {
        public DeveloperRepository(IUnitOfWork<Context> unitOfWork, IDeveloperUserTypeRepository developerUserTypeRepository) : base(unitOfWork, developerUserTypeRepository) { }
    }
}
