using Sindika.AspNet.app015.Application.DTOs.Developer;
using Sindika.AspNet.app015.Application.DTOs.Role;
using Sindika.AspNet.app015.Application.DTOs.Zone;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Services
{
    public class DeveloperRoleService : BaseUserUserTypeService<
        DeveloperRoleService,
        Context,
        RolePaginationDTO,
        DeveloperUserType,
        Developer
        >, IDeveloperRoleService
    {
        public DeveloperRoleService(IConfiguration configuration, ILogger<DeveloperRoleService> logger, IUnitOfWork<Context> unitOfWork, IDeveloperUserTypeRepository repository, IRoleRepository roleRepository) : base(configuration, logger, unitOfWork, repository, roleRepository)
        {

        }
    }
}
