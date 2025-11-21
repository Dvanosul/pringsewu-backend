using Sindika.AspNet.app015.Application.DTOs.Role;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Services
{
    public class EmployeeRoleService : BaseUserUserTypeService<
        EmployeeRoleService,
        Context,
        RolePaginationDTO,
        EmployeeUserType,
        Employee
        >, IEmployeeRoleService
    {
        public EmployeeRoleService(IConfiguration configuration, ILogger<EmployeeRoleService> logger, IUnitOfWork<Context> unitOfWork, IEmployeeUserTypeRepository repository, IRoleRepository roleRepository) : base(configuration, logger, unitOfWork, repository, roleRepository)
        {
        }
    }
}
