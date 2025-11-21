using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories
{
    public interface IProfilePageEventRepository : IBaseRepository<ProfilePageEvent>
    {
        Task<HashSet<Guid>> GetPageEnabledCustomEventIdsAsync(IEnumerable<Guid> profileIds, Guid pageId, IEnumerable<Guid> eventIds);
        Task<List<Guid>> UpsertAsync(Guid roleId, HashSet<(Guid PageId, Guid EventId)> changedPageEvents);
        Task<Dictionary<Guid, long>> GetPagesEnabledCustomEventCountAsync(Guid profileId, IEnumerable<Guid> pageIds);
        Task<Dictionary<Guid, (HashSet<Guid>, long)>> GetPageAccessAndCustomEventCountByProfilesAsync(IEnumerable<Guid> profileIds, IEnumerable<Guid> pageIds);
    }
}
