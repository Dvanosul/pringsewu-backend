using Microsoft.AspNetCore.Http;
using Sindika.AspNet.app015.Application.DTOs.DonationGallery;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IDonationGalleryService : IBaseCrudService<DonationGalleryDTO, DonationGalleryPaginationDTO, DonationGalleryParam>
    {
        Task<List<DonationGalleryDTO>> GetByEventIdAsync(Guid eventId);
        Task<List<DonationGalleryDTO>> GetByEventCodeAsync(string eventCode);
        new Task<Guid> UpdateAsync(DonationGalleryParam param, Guid id);
        Task<Guid> UpdateImageAsync(Guid id, string imgUrl);
        Task<Guid> CreateWithImageAsync(DonationGalleryParam param, IFormFile image);
        Task<Guid> UpdateWithImageAsync(DonationGalleryParam param, Guid id, IFormFile? image);
        Task<(Stream? Stream, string ContentType)> GetImageStreamAsync(Guid id);
        (bool IsValid, string ErrorMessage) ValidateImageFile(IFormFile? file, bool isRequired = true);
    }
}
