using Sindika.AspNet.app015.Application.DTOs.Event;
using Sindika.AspNet.app015.Application.DTOs.Page;
using Sindika.AspNet.app015.Application.DTOs.Role;
using Sindika.AspNet.app015.Application.DTOs.User;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IUserInfoService : IBaseService
    {
        Task<PaginationResponse<UserRolePaginationDTO>> GetUserRolesPaginationAsync(Guid userId, PaginationQuery paginationQuery);
        Task<PaginationResponse<PagePaginationDTO>> GetRolePageAccessPaginationAsync(Guid roleId, PaginationQuery paginationQuery);
        Task<PaginationResponse<EventPaginationDTO>> GetRolePageEventAccessPaginationAsync(Guid roleId, Guid pageId, PaginationQuery paginationQuery);
        Task<UserInfoDTO> GetUserInfo(Guid userId);
    }
}
