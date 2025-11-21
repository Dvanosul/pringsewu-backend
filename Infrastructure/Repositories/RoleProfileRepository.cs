using System.Linq.Expressions;
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
    public class RoleProfileRepository : BaseRepository<RoleProfile, Context>, IRoleProfileRepository
    {
        public RoleProfileRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }

        public async Task<HashSet<Guid>> GetUserProfileIdsAsync(Guid userId, Guid? roleActiveId = null)
        {
            var query = _dbSet.Where(rp => rp.IsActive && _unitOfWork.GetContext().Set<UserUserType>()
                .Where(up => up.IsActive && up.UserId == userId)
                .Select(up => up.RoleId)
                .Contains(rp.RoleId));

            if (roleActiveId.HasValue)
            {
                query = query.Where(c => c.RoleId == roleActiveId.Value);
            }

            return await query
                .Select(rp => rp.ProfileId)
                .ToHashSetAsync();
        }

        public async Task<List<Guid>> UpsertAsync(Guid roleId, List<Guid> changedProfile)
        {
            var profiles = await GetsAsync(rp => rp.Where(c => c.RoleId == roleId));

            var addedProfileIds = changedProfile.Except(profiles.Select(c => c.ProfileId));

            var profilesToAdd = addedProfileIds.Select(c => new RoleProfile { RoleId = roleId, ProfileId = c });
            var profilesToRemove = profiles.Where(p => changedProfile.Contains(p.ProfileId)).Select(c =>
            {
                c.IsActive = false;
                c.UpdatedDate = DateTimeOffset.UtcNow;
                return c;
            });

            _dbSet.AddRange(profilesToAdd);
            _dbSet.UpdateRange(profilesToRemove);

            await _unitOfWork.GetContext().SaveChangesAsync();

            return [.. addedProfileIds, .. profilesToRemove.Select(rp => rp.ProfileId)];
        }

        public Expression<Func<ProfilePageEvent, bool>> RoleProfileQuery(IEnumerable<Guid> roleIds)
        {
            return x => _dbSet.Where(c => c.IsActive && roleIds.Contains(c.RoleId) && c.ProfileId == x.ProfileId).Any();
        }

        public async Task<Dictionary<Guid, IEnumerable<string>>> GetRolesProfilesAsync(IEnumerable<Guid> roleIds)
        {
            var query = _dbSet.Where(c => c.IsActive && roleIds.Contains(c.RoleId))
                              .GroupBy(c => c.RoleId)
                              .Select(c => new { RoleId = c.Key, Profiles = c.Select(rp => rp.Profile!.Name) });

            return await query.ToDictionaryAsync(c => c.RoleId, c => c.Profiles);
        }

        public async Task<Dictionary<Guid, IEnumerable<string>>> GetRolesPageAccessPreviewAsync(IEnumerable<Guid> roleIds)
        {
            var query = _dbSet.Where(c => c.IsActive && roleIds.Contains(c.RoleId))
                .GroupBy(c => c.RoleId)
                .Select(c => new
                {
                    RoleId = c.Key,
                    PageAccess = _unitOfWork.GetContext().ProfilePageEvents
                                             .Where(ppe => ppe.IsActive && c.Select(rp => rp.ProfileId).Contains(ppe.ProfileId))
                                             .Select(ppe => ppe.Page!.Name)
                                             .Distinct().Take(5).AsEnumerable()
                });

            return await query.ToDictionaryAsync(c => c.RoleId, c => c.PageAccess);
        }

        public async Task<ItemCountResponse<Page>> GetRolePageAccessPaginationAsync(Guid roleId, PaginationQuery paginationQuery, Func<IQueryable<Page>, IQueryable<Page>>? include = null)
        {
            var source = _unitOfWork.GetContext().ProfilePageEvents
                                    .AsQueryable()
                                    .Where((c) => c.IsActive && _dbSet.Where(rp => rp.IsActive && rp.RoleId == roleId)
                                                                      .Select(rp => rp.ProfileId)
                                                                      .Contains(c.ProfileId))
                                    .Select(c => c.Page!)
                                    .Distinct();

            if (include != null)
            {
                source = include(source);
            }

            if (paginationQuery.Filters.Any())
            {
                source = source.Where(BuildDynamicFilter<Page>(paginationQuery));
            }

            paginationQuery.AddDefaultSort("createddate", "desc");
            return await BuildPaginationQuery(paginationQuery, source);
        }

        public async Task<ItemCountResponse<Event>> GetRolePageEventAccessPaginationAsync(Guid roleId, Guid pageId, PaginationQuery paginationQuery, Func<IQueryable<Event>, IQueryable<Event>>? include = null)
        {
            var source = _unitOfWork.GetContext().ProfilePageEvents
                                    .AsQueryable()
                                    .Where((c) => c.IsActive && c.PageId == pageId && _dbSet.Where(rp => rp.IsActive && rp.RoleId == roleId)
                                                                      .Select(rp => rp.ProfileId)
                                                                      .Contains(c.ProfileId))
                                    .Select(c => c.Event!)
                                    .Distinct();

            if (include != null)
            {
                source = include(source);
            }

            if (paginationQuery.Filters.Any())
            {
                source = source.Where(BuildDynamicFilter<Event>(paginationQuery));
            }

            paginationQuery.AddDefaultSort("createddate", "desc");
            return await BuildPaginationQuery(paginationQuery, source);
        }
    }
}
