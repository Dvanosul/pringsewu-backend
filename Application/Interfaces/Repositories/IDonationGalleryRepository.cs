using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories
{
    public interface IDonationGalleryRepository : IBaseRepository<DonationGallery>
    {
        Task<List<DonationGallery>> GetByEventIdAsync(Guid eventId);
        Task<List<DonationGallery>> GetByEventCodeAsync(string eventCode);
    }
}
