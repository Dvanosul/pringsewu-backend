using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories
{
    public interface IPageEventRepository : IBaseRepository<PageEvent>
    {
        Task<ItemCountResponse<Event>> GetCustomEventPaginationAsync(Guid pageId, PaginationQuery paginationQuery, Func<IQueryable<Event>, IQueryable<Event>>? include = null);
        public Task<Dictionary<Guid, long>> GetPagesCustomEventCount(IEnumerable<Guid> pageIds);
        public Task<Dictionary<Guid, int>> GetEventsAppliedPageCount(IEnumerable<Guid> eventIds);
    }
}
