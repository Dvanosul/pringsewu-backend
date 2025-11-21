using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Repositories;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class UserUserTypeRepository : BaseRepository<UserUserType, Context>, IUserUserTypeRepository
    {
        public UserUserTypeRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }

        public async Task<ItemCountResponse<UserUserType>> GetUserRolesPaginationAsync(Guid userId, PaginationQuery paginationQuery, Func<IQueryable<UserUserType>, IQueryable<UserUserType>>? include = null)
        {
            var source = _dbSet.AsQueryable();
            source = source.Where((c) => c.IsActive && c.UserId == userId && c.IsEnabled && c.RoleId.HasValue);

            if (include != null)
            {
                source = include(source);
            }

            if (paginationQuery.Filters.Any())
            {
                source = source.Where(BuildDynamicFilter<UserUserType>(paginationQuery));
            }

            paginationQuery.AddDefaultSort("createddate", "desc");
            return await BuildPaginationQuery(paginationQuery, source.Include("Role"));
        }

        public async Task<List<Role>> GetUserRolesAsync(Guid userId)
        {
            return await _dbSet.Where(uut => uut.IsActive && uut.UserId == userId)
                               .Include("Role")
                               .Select(uut => uut.Role!)
                               .ToListAsync();
        }

        public async Task<Dictionary<string, IEnumerable<Role>>> GetUserTypesRolesAsync(Guid userId)
        {
            var query = _dbSet.Where(uut => uut.IsActive && uut.UserId == userId).Include("UserType").Include("Role")
                              .GroupBy(uut => uut.UserTypeId)
                              .Select(c => new { UserType = c.First().UserType!.Code, Roles = c.Select(uut => uut.Role!) });

            return await query.ToDictionaryAsync(c => c.UserType, c => c.Roles);
        }

        public async Task<Dictionary<Guid, HashSet<string>>> GetUsersUserTypesAsync(IEnumerable<Guid> userIds)
        {
            return await _dbSet.Where(uut => uut.IsActive && uut.UserId != null && userIds.Contains(uut.UserId.Value) && uut.IsEnabled)
                               .Include("UserType")
                               .Select(uut => new { UserId = uut.UserId!.Value, UserType = uut.UserType!.Code })
                               .GroupBy(c => c.UserId)
                               .ToDictionaryAsync(c => c.Key, c => c.Select(d => d.UserType).ToHashSet());
        }
    }
}
