using System.Text;
using System.Text.Json;
using Sindika.AspNet.app015.API.Models.Blockchain;
using Sindika.AspNet.app015.Application.DTOs.Blockchain.Withdrawal;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Services.Blockchain
{
    public class BlockchainWithdrawalService : IBlockchainWithdrawalService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BlockchainWithdrawalService> _logger;
        private readonly ICacheService _cacheService;
        private readonly string _baseUrl;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public BlockchainWithdrawalService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BlockchainWithdrawalService> logger,
            ICacheService cacheService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _cacheService = cacheService;
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
                var result = JsonSerializer.Deserialize<BlockchainWithdrawalSingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainWithdrawalSingleResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success)
                {
                    await InvalidateWithdrawalCache(request.EventCode);
                    _logger.LogInformation("Invalidated withdrawal cache after creating new withdrawal");
                }

                return result;
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




        private async Task InvalidateWithdrawalCache(string? eventCode = null)
        {
            try
            {
                await _cacheService.HashRemoveAsync("blockchain:withdrawals", "all");
                await _cacheService.HashRemoveAsync("blockchain:withdrawals", "total");

                if (!string.IsNullOrEmpty(eventCode))
                {
                    await _cacheService.HashRemoveAsync("blockchain:withdrawals", $"event:{eventCode}");
                    await _cacheService.HashRemoveAsync("blockchain:withdrawals", $"event:{eventCode}:total");
                }

                _logger.LogInformation("Successfully invalidated withdrawal cache");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to invalidate withdrawal cache, but continuing operation");
            }
        }

        public async Task<BlockchainWithdrawalSingleResponse> GetWithdrawalAsync(string id)
        {
            try
            {
                var cacheField = $"withdrawal:{id}";
                var isExist = await _cacheService.IsExistHashAsync("blockchain:withdrawals", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved withdrawal {WithdrawalId} from cache", id);
                }

                _logger.LogInformation("Fetching withdrawal {WithdrawalId} from API", id);
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/withdrawals/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainWithdrawalSingleResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainWithdrawalSingleResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success && result.Data != null)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:withdrawals", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached withdrawal {WithdrawalId} for {Minutes} minutes", id, _cacheExpiration.TotalMinutes);
                }

                return result;
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
                var cacheField = "all";
                var isExist = await _cacheService.IsExistHashAsync("blockchain:withdrawals", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved all withdrawals from cache");
                }

                _logger.LogInformation("Fetching all withdrawals from API");
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/withdrawals");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainWithdrawalListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainWithdrawalListResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success && result.Data != null)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:withdrawals", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached all withdrawals for {Minutes} minutes", _cacheExpiration.TotalMinutes);
                }

                return result;
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
                var cacheField = "total";
                var isExist = await _cacheService.IsExistHashAsync("blockchain:withdrawals", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved total withdrawals from cache");
                }

                _logger.LogInformation("Fetching total withdrawals from API");
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/withdrawals/total");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainTotalWithdrawalResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainTotalWithdrawalResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:withdrawals", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached total withdrawals for {Minutes} minutes", _cacheExpiration.TotalMinutes);
                }

                return result;
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
                var cacheField = $"event:{eventCode}";
                var isExist = await _cacheService.IsExistHashAsync("blockchain:withdrawals", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved withdrawals for event {EventCode} from cache", eventCode);
                }

                _logger.LogInformation("Fetching withdrawals for event {EventCode} from API", eventCode);
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/withdrawals/event/{eventCode}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainWithdrawalListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainWithdrawalListResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success && result.Data != null)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:withdrawals", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached withdrawals for event {EventCode} for {Minutes} minutes", eventCode, _cacheExpiration.TotalMinutes);
                }

                return result;
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
                var cacheField = $"event:{eventCode}:total";
                var isExist = await _cacheService.IsExistHashAsync("blockchain:withdrawals", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved total withdrawals for event {EventCode} from cache", eventCode);
                }

                _logger.LogInformation("Fetching total withdrawals for event {EventCode} from API", eventCode);
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/withdrawals/event/{eventCode}/total");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainTotalWithdrawalResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainTotalWithdrawalResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:withdrawals", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached total withdrawals for event {EventCode} for {Minutes} minutes", eventCode, _cacheExpiration.TotalMinutes);
                }

                return result;
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
