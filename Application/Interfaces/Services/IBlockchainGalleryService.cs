using Sindika.AspNet.app015.API.Models.Blockchain;
using Sindika.AspNet.app015.Application.DTOs.Blockchain.Gallery;

namespace Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain
{
    public interface IBlockchainGalleryService
    {
        Task<BlockchainGallerySingleResponse> CreateGalleryAsync(CreateBlockchainGalleryRequest request);
        Task<BlockchainGallerySingleResponse> GetGalleryAsync(string id);
        Task<BlockchainGalleryListResponse> GetAllGalleriesAsync();
        Task<BlockchainGalleryListResponse> GetGalleriesByEventCodeAsync(string eventCode);
        Task<BlockchainGallerySingleResponse> UpdateGalleryAsync(string id, UpdateBlockchainGalleryRequest request);
        Task<BlockchainGalleryResponse> DeleteGalleryAsync(string id);
    }
}
