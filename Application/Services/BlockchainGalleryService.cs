using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
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
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxFileSize = 5 * 1024 * 1024; 

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
                var validationResult = ValidateImageFile(request.Image);
                if (!validationResult.IsValid)
                {
                    return new BlockchainGallerySingleResponse
                    {
                        Success = false,
                        Message = validationResult.ErrorMessage
                    };
                }

                var galleryId = Guid.NewGuid().ToString();

                using var content = new MultipartFormDataContent();
                content.Add(new StringContent(galleryId), "id");
                content.Add(new StringContent(request.EventCode), "eventCode");
                content.Add(new StringContent(request.Description ?? string.Empty), "description");

                using var stream = request.Image.OpenReadStream();
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.Image.ContentType);
                content.Add(fileContent, "image", request.Image.FileName);

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/gallery", content);
                
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainGallerySingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGallerySingleResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success)
                {
                    await InvalidateGalleryCache(request.EventCode);
                    _logger.LogInformation("Created gallery {GalleryId} with image upload", galleryId);
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

        private (bool IsValid, string ErrorMessage) ValidateImageFile(IFormFile? file, bool isRequired = true)
        {
            if (file == null || file.Length == 0)
            {
                return isRequired ? (false, "Image file is required") : (true, string.Empty);
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
            {
                return (false, $"Invalid file type. Allowed: {string.Join(", ", _allowedExtensions)}");
            }

            if (file.Length > MaxFileSize)
            {
                return (false, "File size exceeds 5MB limit");
            }

            return (true, string.Empty);
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

        public async Task<BlockchainGalleryImageResponse> GetGalleryImageAsync(string id)
        {
            try
            {
                _logger.LogInformation("Fetching gallery image {GalleryId} from API", id);
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/gallery/{id}/image");

                if (!response.IsSuccessStatusCode)
                {
                    return new BlockchainGalleryImageResponse
                    {
                        Success = false,
                        Message = "Image not found"
                    };
                }

                var imageData = await response.Content.ReadAsByteArrayAsync();
                var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";

                return new BlockchainGalleryImageResponse
                {
                    Success = true,
                    Message = "Image retrieved successfully",
                    ImageData = imageData,
                    ContentType = contentType
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting gallery image {GalleryId} from blockchain", id);
                return new BlockchainGalleryImageResponse
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
                if (request.Image != null)
                {
                    var validationResult = ValidateImageFile(request.Image, isRequired: false);
                    if (!validationResult.IsValid)
                    {
                        return new BlockchainGallerySingleResponse
                        {
                            Success = false,
                            Message = validationResult.ErrorMessage
                        };
                    }
                }

                using var content = new MultipartFormDataContent();
                content.Add(new StringContent(request.EventCode), "eventCode");
                content.Add(new StringContent(request.Description ?? string.Empty), "description");

                if (request.Image != null)
                {
                    using var stream = request.Image.OpenReadStream();
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    var fileContent = new StreamContent(memoryStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.Image.ContentType);
                    content.Add(fileContent, "image", request.Image.FileName);
                }

                var response = await _httpClient.PutAsync($"{_baseUrl}/api/gallery/{id}", content);
                
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainGallerySingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainGallerySingleResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success)
                {
                    await InvalidateGalleryCache(request.EventCode);
                    await _cacheService.HashRemoveAsync("blockchain:galleries", $"gallery:{id}");
                    _logger.LogInformation("Updated gallery {GalleryId}", id);
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
