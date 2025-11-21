using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Repositories;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class ProfilePageEventRepository : BaseRepository<ProfilePageEvent, Context>, IProfilePageEventRepository
    {
        public ProfilePageEventRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }

        public async Task<HashSet<Guid>> GetPageEnabledCustomEventIdsAsync(IEnumerable<Guid> profileIds, Guid pageId, IEnumerable<Guid> eventIds)
        {
            return await _dbSet.Where(c => c.IsActive
                                           && !c.Event!.IsDefault
                                           && profileIds.Contains(c.ProfileId)
                                           && c.PageId == pageId
                                           && eventIds.Contains(c.EventId))
                .Select(c => c.EventId)
                .ToHashSetAsync();
        }

        public async Task<Dictionary<Guid, long>> GetPagesEnabledCustomEventCountAsync(Guid profileId, IEnumerable<Guid> pageIds)
        {
            var query = _dbSet.Where(c => c.IsActive && !c.Event!.IsDefault && c.ProfileId == profileId && pageIds.Contains(c.PageId))
                .GroupBy(c => c.PageId)
                .Select(c => new { PageId = c.Key, Count = c.Select(pe => pe.EventId).LongCount() });

            return await query.ToDictionaryAsync(c => c.PageId, c => c.Count);
        }

        public async Task<List<Guid>> UpsertAsync(Guid profileId, HashSet<(Guid PageId, Guid EventId)> changedPageEvents)
        {
            var pageIds = changedPageEvents.Select(c => c.PageId);
            var eventIds = changedPageEvents.Select(c => c.EventId);

            var profilePageEvents = await GetsAsync(q => q.Where(ppe => ppe.ProfileId == profileId
                                                              && pageIds.Contains(ppe.PageId)
                                                              && eventIds.Contains(ppe.EventId)));
            var profilePageEventHashSet = new HashSet<(Guid PageId, Guid EventId)>(profilePageEvents.Select(ppe => (ppe.PageId, ppe.EventId)));

            var addedProfilePageEventIds = changedPageEvents.Except(profilePageEventHashSet);

            var profilesToAdd = addedProfilePageEventIds.Select(c => new ProfilePageEvent { ProfileId = profileId, PageId = c.PageId, EventId = c.EventId });
            var profilesToRemove = profilePageEvents.ExceptBy(addedProfilePageEventIds, ppe => (ppe.PageId, ppe.EventId)).Select(c =>
            {
                c.IsActive = false;
                c.UpdatedDate = DateTimeOffset.UtcNow;
                return c;
            });

            _dbSet.AddRange(profilesToAdd);
            _dbSet.UpdateRange(profilesToRemove);

            await _unitOfWork.GetContext().SaveChangesAsync();

            return [.. profilesToAdd.Select(ppe => ppe.EventId), .. profilesToRemove.Select(ppe => ppe.EventId)];
        }

        public async Task<Dictionary<Guid, (HashSet<Guid>, long)>> GetPageAccessAndCustomEventCountByProfilesAsync(IEnumerable<Guid> profileIds, IEnumerable<Guid> pageIds)
        {
            var query = _dbSet.Where(c => c.IsActive && profileIds.Contains(c.ProfileId) && pageIds.Contains(c.PageId))
                .GroupBy(c => c.PageId)
                .Select(c => new
                {
                    PageId = c.Key,
                    Events = c.Where(c => c.Event!.IsDefault).Select(c => c.EventId).ToHashSet(),
                    CustomEventCount = c.Where(c => !c.Event!.IsDefault).Select(c => c.EventId).Distinct().LongCount()
                });

            return await query.ToDictionaryAsync(c => c.PageId, c => (c.Events, c.CustomEventCount));
        }
    }
}
