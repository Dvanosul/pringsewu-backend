using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Builders;
using Sindika.AspNet.QueryBuilder.Repositories;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class DonationEventRepository : BaseRepository<DonationEvent, Context>, IDonationEventRepository
    {
        public DonationEventRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }

        public async Task<DonationEvent?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .TagWithCallSiteGlobal()
                .FirstOrDefaultAsync(e => e.IsActive && e.Code == code);
        }

        public async Task<List<DonationEvent>> GetActiveEventsAsync()
        {
            var now = DateTimeOffset.UtcNow;
            return await _dbSet
                .TagWithCallSiteGlobal()
                .Where(e => e.IsActive && e.StartDate <= now && e.EndDate >= now)
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }

        public async Task<bool> IsCodeExistAsync(string code, Guid? excludeId = null)
        {
            var query = _dbSet.TagWithCallSiteGlobal().Where(e => e.IsActive && e.Code == code);

            if (excludeId.HasValue)
            {
                query = query.Where(e => e.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
