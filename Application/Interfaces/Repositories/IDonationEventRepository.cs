using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Repositories
{
    public interface IDonationEventRepository : IBaseRepository<DonationEvent>
    {
        Task<DonationEvent?> GetByCodeAsync(string code);
        Task<List<DonationEvent>> GetActiveEventsAsync();
        Task<bool> IsCodeExistAsync(string code, Guid? excludeId = null);
    }
}
