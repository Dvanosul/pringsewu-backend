using Sindika.AspNet.app015.Application.DTOs.Page;
using Sindika.AspNet.app015.Application.DTOs.Role;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IRoleService : IBaseCrudService<RoleDTO, RolePaginationDTO, RoleParam>
    {
        Task<List<RoleDTO>> GetsByCodeAsync(List<string>? codes = null);
        Task<bool> SynchronizeRoles(List<string> rolesToAdd, List<string> rolesToRemove);
    }
}
