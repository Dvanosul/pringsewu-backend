using Sindika.AspNet.app015.Application.DTOs.Role;
using Sindika.AspNet.app015.Application.DTOs.RoleProfile;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IRoleProfileService : IBaseCrudService<RoleProfileDTO, RoleProfilePaginationDTO, RoleProfileParam>
    {
        public Task<PaginationResponse<RoleProfilePaginationDTO>> GetCustomPaginationAsync(Guid roleId, PaginationQuery paginationQuery);
        public Task UpsertAsync(RoleProfileParam param);
    }
}
