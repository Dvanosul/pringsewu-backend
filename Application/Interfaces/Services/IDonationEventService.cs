using Microsoft.AspNetCore.Http;
using Sindika.AspNet.app015.Application.DTOs.DonationEvent;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IDonationEventService : IBaseCrudService<DonationEventDTO, DonationEventPaginationDTO, CreateDonationEventParam>
    {
        Task<List<DonationEventDTO>> GetActiveEventsAsync();
        Task<Guid> UpdateStatusAsync(UpdateDonationEventStatusParam param, Guid id);
        Task<Guid> CreateWithImageAsync(CreateDonationEventParam param, IFormFile image);
        Task<Guid> UpdateWithImageAsync(UpdateDonationEventParam param, Guid id, IFormFile? image);
        Task<(Stream? Stream, string ContentType)> GetImageStreamAsync(Guid id);
        (bool IsValid, string ErrorMessage) ValidateImageFile(IFormFile? file, bool isRequired = true);
    }
}
