using System.Text;
using System.Text.Json;
using Sindika.AspNet.app015.API.Models.Blockchain;
using Sindika.AspNet.app015.Application.DTOs.Blockchain.Gallery;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;

namespace Sindika.AspNet.app015.Application.Services.Blockchain
{
    public class BlockchainGalleryService : IBlockchainGalleryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BlockchainGalleryService> _logger;
        private readonly string _baseUrl;

        public BlockchainGalleryService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BlockchainGalleryService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _baseUrl = configuration["VaFundApi:BaseUrl"] ?? "http://localhost:3000";
        }

        public async Task<BlockchainGallerySingleResponse> CreateGalleryAsync(CreateBlockchainGalleryRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/api/gallery", content);
                
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainGallerySingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGallerySingleResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating gallery in blockchain");
                return new BlockchainGallerySingleResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainGallerySingleResponse> GetGalleryAsync(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/gallery/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainGallerySingleResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGallerySingleResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting gallery {GalleryId} from blockchain", id);
                return new BlockchainGallerySingleResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainGalleryListResponse> GetAllGalleriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/gallery");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainGalleryListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGalleryListResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all galleries from blockchain");
                return new BlockchainGalleryListResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainGalleryListResponse> GetGalleriesByEventCodeAsync(string eventCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/gallery/event/{eventCode}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainGalleryListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGalleryListResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting galleries by event code from blockchain");
                return new BlockchainGalleryListResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainGallerySingleResponse> UpdateGalleryAsync(string id, UpdateBlockchainGalleryRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_baseUrl}/api/gallery/{id}", content);
                
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainGallerySingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGallerySingleResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating gallery in blockchain");
                return new BlockchainGallerySingleResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainGalleryResponse> DeleteGalleryAsync(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/gallery/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainGalleryResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGalleryResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting gallery from blockchain");
                return new BlockchainGalleryResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
