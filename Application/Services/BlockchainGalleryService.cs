using System.Text;
using System.Text.Json;
using Sindika.AspNet.app015.API.Models.Blockchain;
using Sindika.AspNet.app015.Application.DTOs.Blockchain.Gallery;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Services.Blockchain
{
    public class BlockchainGalleryService : IBlockchainGalleryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BlockchainGalleryService> _logger;
        private readonly ICacheService _cacheService;
        private readonly string _baseUrl;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public BlockchainGalleryService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BlockchainGalleryService> logger,
            ICacheService cacheService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _cacheService = cacheService;
            _baseUrl = configuration["VaFundApi:BaseUrl"] ?? "http://localhost:3000";
        }

        public async Task<BlockchainGallerySingleResponse> CreateGalleryAsync(CreateBlockchainGalleryRequest request)
        {
            try
            {
                var galleryId = Guid.NewGuid().ToString();
                var requestWithId = new
                {
                    id = galleryId,
                    eventCode = request.EventCode,
                    imageURL = request.ImageURL,
                    description = request.Description
                };
                var json = JsonSerializer.Serialize(requestWithId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/api/gallery", content);
                
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainGallerySingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGallerySingleResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success)
                {
                    await InvalidateGalleryCache(request.EventCode);
                    _logger.LogInformation("Invalidated gallery cache after creating new gallery");
                }

                return result;
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




        private async Task InvalidateGalleryCache(string? eventCode = null)
        {
            try
            {
                await _cacheService.HashRemoveAsync("blockchain:galleries", "all");

                if (!string.IsNullOrEmpty(eventCode))
                {
                    await _cacheService.HashRemoveAsync("blockchain:galleries", $"event:{eventCode}");
                }

                _logger.LogInformation("Successfully invalidated gallery cache");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to invalidate gallery cache, but continuing operation");
            }
        }

        public async Task<BlockchainGallerySingleResponse> GetGalleryAsync(string id)
        {
            try
            {
                var cacheField = $"gallery:{id}";
                var isExist = await _cacheService.IsExistHashAsync("blockchain:galleries", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved gallery {GalleryId} from cache", id);
                }

                _logger.LogInformation("Fetching gallery {GalleryId} from API", id);
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/gallery/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainGallerySingleResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGallerySingleResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success && result.Data != null)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:galleries", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached gallery {GalleryId} for {Minutes} minutes", id, _cacheExpiration.TotalMinutes);
                }

                return result;
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
                var cacheField = "all";
                var isExist = await _cacheService.IsExistHashAsync("blockchain:galleries", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved all galleries from cache");
                }

                _logger.LogInformation("Fetching all galleries from API");
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/gallery");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainGalleryListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGalleryListResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success && result.Data != null)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:galleries", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached all galleries for {Minutes} minutes", _cacheExpiration.TotalMinutes);
                }

                return result;
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
                var cacheField = $"event:{eventCode}";
                var isExist = await _cacheService.IsExistHashAsync("blockchain:galleries", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved galleries for event {EventCode} from cache", eventCode);
                }

                _logger.LogInformation("Fetching galleries for event {EventCode} from API", eventCode);
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/gallery/event/{eventCode}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainGalleryListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGalleryListResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success && result.Data != null)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:galleries", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached galleries for event {EventCode} for {Minutes} minutes", eventCode, _cacheExpiration.TotalMinutes);
                }

                return result;
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
                var result = JsonSerializer.Deserialize<BlockchainGallerySingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGallerySingleResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success)
                {
                    await InvalidateGalleryCache(request.EventCode);
                    await _cacheService.HashRemoveAsync("blockchain:galleries", $"gallery:{id}");
                    _logger.LogInformation("Invalidated gallery cache after updating");
                }

                return result;
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
                var result = JsonSerializer.Deserialize<BlockchainGalleryResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGalleryResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success)
                {
                    await InvalidateGalleryCache();
                    await _cacheService.HashRemoveAsync("blockchain:galleries", $"gallery:{id}");
                    _logger.LogInformation("Invalidated gallery cache after deleting");
                }

                return result;
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
