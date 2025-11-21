using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories
{
    public interface IUserUserTypeRepository : IBaseRepository<UserUserType>
    {
        Task<Dictionary<string, IEnumerable<Role>>> GetUserTypesRolesAsync(Guid userId);
        Task<ItemCountResponse<UserUserType>> GetUserRolesPaginationAsync(Guid userId, PaginationQuery paginationQuery, Func<IQueryable<UserUserType>, IQueryable<UserUserType>>? include = null);
        Task<List<Role>> GetUserRolesAsync(Guid userId);
        Task<Dictionary<Guid, HashSet<string>>> GetUsersUserTypesAsync(IEnumerable<Guid> userIds);
    }
}
