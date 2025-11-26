using System.Text;
using System.Text.Json;
using Sindika.AspNet.app015.API.Models.Blockchain;
using Sindika.AspNet.app015.Application.DTOs.Blockchain.Event;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Services.Blockchain
{
    public class BlockchainEventService : IBlockchainEventService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BlockchainEventService> _logger;
        private readonly ICacheService _cacheService;
        private readonly string _baseUrl;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public BlockchainEventService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BlockchainEventService> logger,
            ICacheService cacheService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _cacheService = cacheService;
            _baseUrl = configuration["VaFundApi:BaseUrl"] ?? "http://localhost:3000";
        }

        public async Task<BlockchainEventSingleResponse> CreateEventAsync(CreateBlockchainEventRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/api/events", content);
                
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainEventSingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainEventSingleResponse { Success = false, Message = "Failed to deserialize response" };

               
                if (result.Success)
                {
                    await InvalidateEventCache(request.Code);
                    _logger.LogInformation("Invalidated event cache after creating new event");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event in blockchain");
                return new BlockchainEventSingleResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }




        private async Task InvalidateEventCache(string? eventCode = null)
        {
            try
            {
                await _cacheService.HashRemoveAsync("blockchain:events", "all");
                await _cacheService.HashRemoveAsync("blockchain:events", "active");

                if (!string.IsNullOrEmpty(eventCode))
                {
                    await _cacheService.HashRemoveAsync("blockchain:events", $"event:{eventCode}");
                }

                _logger.LogInformation("Successfully invalidated event cache");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to invalidate event cache, but continuing operation");
            }
        }

        public async Task<BlockchainEventSingleResponse> GetEventAsync(string code)
        {
            try
            {
                var cacheField = $"event:{code}";
                var isExist = await _cacheService.IsExistHashAsync("blockchain:events", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved event {EventCode} from cache", code);
                }

                _logger.LogInformation("Fetching event {EventCode} from API", code);
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/events/{code}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainEventSingleResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainEventSingleResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success && result.Data != null)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:events", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached event {EventCode} for {Minutes} minutes", code, _cacheExpiration.TotalMinutes);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting event {EventCode} from blockchain", code);
                return new BlockchainEventSingleResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainEventListResponse> GetAllEventsAsync()
        {
            try
            {
                var cacheField = "all";
                var isExist = await _cacheService.IsExistHashAsync("blockchain:events", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved all events from cache");
                }

                _logger.LogInformation("Fetching all events from API");
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/events");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainEventListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainEventListResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success && result.Data != null)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:events", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached all events for {Minutes} minutes", _cacheExpiration.TotalMinutes);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all events from blockchain");
                return new BlockchainEventListResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainEventListResponse> GetActiveEventsAsync()
        {
            try
            {
                var cacheField = "active";
                var isExist = await _cacheService.IsExistHashAsync("blockchain:events", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved active events from cache");
                }

                _logger.LogInformation("Fetching active events from API");
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/events/active");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainEventListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainEventListResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success && result.Data != null)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:events", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached active events for {Minutes} minutes", _cacheExpiration.TotalMinutes);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active events from blockchain");
                return new BlockchainEventListResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainEventSingleResponse> UpdateEventStatusAsync(string code, UpdateBlockchainEventStatusRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_baseUrl}/api/events/{code}/status", content);
                
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainEventSingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainEventSingleResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success)
                {
                    await InvalidateEventCache(code);
                    _logger.LogInformation("Invalidated event cache after updating status");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating event status in blockchain");
                return new BlockchainEventSingleResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainEventSingleResponse> UpdateEventAsync(string code, UpdateBlockchainEventRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_baseUrl}/api/events/{code}", content);
                
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainEventSingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainEventSingleResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success)
                {
                    await InvalidateEventCache(code);
                    _logger.LogInformation("Invalidated event cache after updating");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating event in blockchain");
                return new BlockchainEventSingleResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
