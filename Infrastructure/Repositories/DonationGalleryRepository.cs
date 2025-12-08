using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.QueryBuilder.Builders;
using Sindika.AspNet.QueryBuilder.Repositories;

namespace Sindika.AspNet.app015.Infrastructure.Repositories
{
    public class DonationGalleryRepository : BaseRepository<DonationGallery, Context>, IDonationGalleryRepository
    {
        public DonationGalleryRepository(IUnitOfWork<Context> unitOfWork) : base(unitOfWork) { }

        public async Task<List<DonationGallery>> GetByEventIdAsync(Guid DonationEventId)
        {
            return await _dbSet
                .TagWithCallSiteGlobal()
                .Include(g => g.DonationEvent)
                .Where(g => g.IsActive && g.DonationEventId == DonationEventId)
                .OrderByDescending(g => g.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<DonationGallery>> GetByEventCodeAsync(string eventCode)
        {
            return await _dbSet
                .TagWithCallSiteGlobal()
                .Include(g => g.DonationEvent)
                .Where(g => g.IsActive && g.DonationEvent != null && g.DonationEvent.Code == eventCode)
                .OrderByDescending(g => g.CreatedDate)
                .ToListAsync();
        }
    }
}
