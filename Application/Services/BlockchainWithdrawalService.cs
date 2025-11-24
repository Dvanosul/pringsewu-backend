using System.Text;
using System.Text.Json;
using Sindika.AspNet.app015.Application.DTOs.Blockchain;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;

namespace Sindika.AspNet.app015.Application.Services.Blockchain
{
    public class BlockchainWithdrawalService : IBlockchainWithdrawalService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BlockchainWithdrawalService> _logger;
        private readonly string _baseUrl;

        public BlockchainWithdrawalService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BlockchainWithdrawalService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _baseUrl = configuration["VaFundApi:BaseUrl"] ?? "http://localhost:3000";
        }

        public async Task<BlockchainWithdrawalSingleResponse> CreateWithdrawalAsync(CreateBlockchainWithdrawalRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/api/withdrawals", content);
                
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainWithdrawalSingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainWithdrawalSingleResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating withdrawal in blockchain");
                return new BlockchainWithdrawalSingleResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainWithdrawalSingleResponse> GetWithdrawalAsync(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/withdrawals/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainWithdrawalSingleResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainWithdrawalSingleResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting withdrawal {WithdrawalId} from blockchain", id);
                return new BlockchainWithdrawalSingleResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainWithdrawalListResponse> GetAllWithdrawalsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/withdrawals");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainWithdrawalListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainWithdrawalListResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all withdrawals from blockchain");
                return new BlockchainWithdrawalListResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainTotalWithdrawalResponse> GetTotalWithdrawalsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/withdrawals/total");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainTotalWithdrawalResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainTotalWithdrawalResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total withdrawals from blockchain");
                return new BlockchainTotalWithdrawalResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainWithdrawalListResponse> GetWithdrawalsByEventCodeAsync(string eventCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/withdrawals/event/{eventCode}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainWithdrawalListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainWithdrawalListResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting withdrawals by event code from blockchain");
                return new BlockchainWithdrawalListResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BlockchainTotalWithdrawalResponse> GetTotalWithdrawalsByEventCodeAsync(string eventCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/withdrawals/event/{eventCode}/total");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BlockchainTotalWithdrawalResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainTotalWithdrawalResponse { Success = false, Message = "Failed to deserialize response" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total withdrawals by event code from blockchain");
                return new BlockchainTotalWithdrawalResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
