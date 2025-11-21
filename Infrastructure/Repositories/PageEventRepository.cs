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
    public class PageEventRepository : BaseRepository<PageEvent, Context>, IPageEventRepository
    {

        public PageEventRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }

        public async Task<ItemCountResponse<Event>> GetCustomEventPaginationAsync(Guid pageId, PaginationQuery paginationQuery, Func<IQueryable<Event>, IQueryable<Event>>? include = null)
        {
            var source = _dbSet.AsQueryable()
                               .Where((c) => c.IsActive && c.HasEvent && !c.Event!.IsDefault && c.PageId == pageId)
                               .Select(c => c.Event!);

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

        public async Task<Dictionary<Guid, long>> GetPagesCustomEventCount(IEnumerable<Guid> pageIds)
        {
            var query = _dbSet.Where(c => pageIds.Contains(c.PageId) && c.HasEvent && !c.Event!.IsDefault && c.IsActive)
                .GroupBy(c => c.PageId)
                .Select(c => new { PageId = c.Key, Count = c.Select(pe => pe.EventId).LongCount() });

            return await query.ToDictionaryAsync(c => c.PageId, c => c.Count);
        }

        public async Task<Dictionary<Guid, int>> GetEventsAppliedPageCount(IEnumerable<Guid> eventIds)
        {
            var query = _dbSet.Where(c => c.IsActive && eventIds.Contains(c.EventId) && c.HasEvent)
                .GroupBy(c => c.EventId)
                .Select(c => new { EventId = c.Key, Count = c.Count() });

            return await query.ToDictionaryAsync(c => c.EventId, c => c.Count);
        }
    }
}
