using Sindika.AspNet.app015.Application.DTOs.Page;
using Sindika.AspNet.app015.Application.DTOs.Role;
using Sindika.AspNet.app015.Application.DTOs.User;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IUserService : IBaseCrudService<UserDTO, UserPaginationDTO, CreateUserParam>
    {
        Task<Dictionary<string, IEnumerable<RoleWithProfilesDTO>>> GetUserTypesRolesAndProfilesAsync(Guid userId);
        Task<PaginationResponse<PageWithEventsIdPaginationDTO>> GetUserPageAccessPaginationAsync(Guid userId, PaginationQuery paginationQuery);
        Task<PaginationResponse<PageCustomEventDTO>> GetUserPageCustomEventAccessPaginationAsync(Guid userId, Guid pageId, PaginationQuery paginationQuery);
    }
}
