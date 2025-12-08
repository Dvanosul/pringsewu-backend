using Microsoft.AspNetCore.Http;
using Sindika.AspNet.app015.Application.DTOs.DonationEvent;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IDonationEventService : IBaseCrudService<DonationEventDTO, DonationEventPaginationDTO, DonationEventParam>
    {
        Task<List<DonationEventDTO>> GetActiveEventsAsync();
        Task<Guid> UpdateStatusAsync(bool isActive, Guid id);
        Task<Guid> CreateWithImageAsync(DonationEventParam param, IFormFile image);
        Task<Guid> UpdateWithImageAsync(DonationEventParam param, Guid id, IFormFile? image);
        Task<(Stream? Stream, string ContentType)> GetImageStreamAsync(Guid id);
        (bool IsValid, string ErrorMessage) ValidateImageFile(IFormFile? file, bool isRequired = true);
    }
}
