using Microsoft.AspNetCore.Http;
using Sindika.AspNet.app015.Application.DTOs.DonationGallery;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IDonationGalleryService : IBaseCrudService<DonationGalleryDTO, DonationGalleryPaginationDTO, CreateDonationGalleryParam>
    {
        Task<List<DonationGalleryDTO>> GetByEventIdAsync(Guid eventId);
        Task<List<DonationGalleryDTO>> GetByEventCodeAsync(string eventCode);
        Task<Guid> UpdateAsync(UpdateDonationGalleryParam param, Guid id);
        Task<Guid> UpdateImageAsync(Guid id, string imgUrl);
        Task<Guid> CreateWithImageAsync(CreateDonationGalleryParam param, IFormFile image);
        Task<Guid> UpdateWithImageAsync(UpdateDonationGalleryParam param, Guid id, IFormFile? image);
        Task<(Stream? Stream, string ContentType)> GetImageStreamAsync(Guid id);
        (bool IsValid, string ErrorMessage) ValidateImageFile(IFormFile? file, bool isRequired = true);
    }
}
