using System.Text;
using System.Text.Json;
using Sindika.AspNet.app015.Application.DTOs.Blockchain;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;

namespace Sindika.AspNet.app015.Application.Services.Blockchain
{
    public class BlockchainDonationService : IBlockchainDonationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BlockchainDonationService> _logger;
        private readonly string _baseUrl;

        public BlockchainDonationService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BlockchainDonationService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _baseUrl = configuration["VaFundApi:BaseUrl"] ?? "http://localhost:3000";
        }

        public async Task<BlockchainDonationSingleResponse> CreateDonationAsync(CreateBlockchainDonationRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/api/donations", content);
                
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainDonationSingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainDonationSingleResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating donation in blockchain");
                return new BlockchainDonationSingleResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainDonationSingleResponse> GetDonationAsync(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/donations/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainDonationSingleResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainDonationSingleResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting donation {DonationId} from blockchain", id);
                return new BlockchainDonationSingleResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainDonationListResponse> GetAllDonationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/donations");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainDonationListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainDonationListResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all donations from blockchain");
                return new BlockchainDonationListResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainTotalDonationResponse> GetTotalDonationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/donations/total");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainTotalDonationResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainTotalDonationResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total donations from blockchain");
                return new BlockchainTotalDonationResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainDonationListResponse> GetDonationsByAmountAsync(double minAmount)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/donations/by-amount?minAmount={minAmount}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainDonationListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainDonationListResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting donations by amount from blockchain");
                return new BlockchainDonationListResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainDonationListResponse> GetDonationsByEventCodeAsync(string eventCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/donations/event/{eventCode}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainDonationListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainDonationListResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting donations by event code from blockchain");
                return new BlockchainDonationListResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainTotalDonationResponse> GetTotalDonationsByEventCodeAsync(string eventCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/donations/event/{eventCode}/total");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainTotalDonationResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainTotalDonationResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total donations by event code from blockchain");
                return new BlockchainTotalDonationResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
