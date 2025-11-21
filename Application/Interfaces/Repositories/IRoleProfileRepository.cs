using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories
{
    public interface IRoleProfileRepository : IBaseRepository<RoleProfile>
    {
        Task<Dictionary<Guid, IEnumerable<string>>> GetRolesProfilesAsync(IEnumerable<Guid> roleIds);
        Task<HashSet<Guid>> GetUserProfileIdsAsync(Guid userId, Guid? roleActiveId = null);
        Task<List<Guid>> UpsertAsync(Guid roleId, List<Guid> profileIds);
        Task<Dictionary<Guid, IEnumerable<string>>> GetRolesPageAccessPreviewAsync(IEnumerable<Guid> roleIds);
        Task<ItemCountResponse<Page>> GetRolePageAccessPaginationAsync(Guid roleId, PaginationQuery paginationQuery, Func<IQueryable<Page>, IQueryable<Page>>? include = null);
        Task<ItemCountResponse<Event>> GetRolePageEventAccessPaginationAsync(Guid roleId, Guid pageId, PaginationQuery paginationQuery, Func<IQueryable<Event>, IQueryable<Event>>? include = null);
    }
}
