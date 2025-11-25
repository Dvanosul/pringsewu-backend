using System.Text;
using System.Text.Json;
using Sindika.AspNet.app015.API.Models.Blockchain;
using Sindika.AspNet.app015.Application.DTOs.Blockchain.Event;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;

namespace Sindika.AspNet.app015.Application.Services.Blockchain
{
    public class BlockchainEventService : IBlockchainEventService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BlockchainEventService> _logger;
        private readonly string _baseUrl;

        public BlockchainEventService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BlockchainEventService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
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
                return JsonSerializer.Deserialize<BlockchainEventSingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainEventSingleResponse { Success = false, Message = "Failed to deserialize response" };
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

        public async Task<BlockchainEventSingleResponse> GetEventAsync(string code)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/events/{code}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainEventSingleResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainEventSingleResponse { Success = false, Message = "Failed to deserialize response" };
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
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/events");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainEventListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainEventListResponse { Success = false, Message = "Failed to deserialize response" };
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
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/events/active");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainEventListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainEventListResponse { Success = false, Message = "Failed to deserialize response" };
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
                return JsonSerializer.Deserialize<BlockchainEventSingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainEventSingleResponse { Success = false, Message = "Failed to deserialize response" };
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
                return JsonSerializer.Deserialize<BlockchainEventSingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainEventSingleResponse { Success = false, Message = "Failed to deserialize response" };
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
