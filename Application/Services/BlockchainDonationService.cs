using System.Globalization;
using System.Text;
using System.Text.Json;
using Sindika.AspNet.app015.API.Models.Blockchain;
using Sindika.AspNet.app015.Application.DTOs.Blockchain.Donation;
using Sindika.AspNet.app015.Application.DTOs.Midtrans;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Midtrans.Contracts;
using Sindika.AspNet.Midtrans.Models.Request.Snap;
using Sindika.AspNet.Midtrans.Models.Common;
using Sindika.AspNet.Midtrans.Exceptions;

namespace Sindika.AspNet.app015.Application.Services.Blockchain
{
    public class BlockchainDonationService : IBlockchainDonationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BlockchainDonationService> _logger;
        private readonly ICacheService _cacheService;
        private readonly IPendingDonationService _pendingDonationService;
        private readonly IMidtransClient _midtransClient;
        private readonly string _baseUrl;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public BlockchainDonationService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BlockchainDonationService> logger,
            ICacheService cacheService,
            IPendingDonationService pendingDonationService,
            IMidtransClient midtransClient)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _cacheService = cacheService;
            _pendingDonationService = pendingDonationService;
            _midtransClient = midtransClient;
            _baseUrl = configuration["VaFundApi:BaseUrl"] ?? "http://localhost:3000";
        }

        public async Task<PaymentResponseDTO> CreateDonationPaymentAsync(CreateBlockchainDonationRequest request)
        {
            var orderId = Guid.NewGuid().ToString();
            
            var snapRequest = new SnapTransactionRequest
            {
                TransactionDetails = new TransactionDetails
                {
                    OrderId = orderId,
                    GrossAmount = ParseAmount(request.Amount)
                },
                CustomerDetails = new CustomerDetails
                {
                    FirstName = request.SenderName
                }
            };

            var response = await _midtransClient.Snap.CreateTransactionAsync(snapRequest);

            var pendingDonation = new PendingDonationDTO
            {
                DonationId = orderId,
                SenderName = request.SenderName,
                Amount = request.Amount,
                Message = request.Message,
                EventCode = request.EventCode,
                CreatedAt = DateTime.UtcNow
            };

            await _pendingDonationService.StorePendingDonationAsync(orderId, pendingDonation);
            _logger.LogInformation("Created payment for donation {DonationId}, waiting for payment confirmation", orderId);

            return new PaymentResponseDTO
            {
                Token = response.Token,
                RedirectUrl = response.RedirectUrl
            };
        }

        public async Task<BlockchainDonationSingleResponse> CreateDonationInBlockchainAsync(PendingDonationDTO pendingDonation)
        {
            try
            {
                var requestWithId = new
                {
                    donationId = pendingDonation.DonationId,
                    senderName = pendingDonation.SenderName,
                    amount = pendingDonation.Amount,
                    message = pendingDonation.Message,
                    eventCode = pendingDonation.EventCode
                };

                var json = JsonSerializer.Serialize(requestWithId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/api/donations", content);
                
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainDonationSingleResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainDonationSingleResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success)
                {
                    await InvalidateDonationCache(pendingDonation.EventCode);
                    _logger.LogInformation("Successfully created donation {DonationId} in blockchain after payment confirmation", pendingDonation.DonationId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating donation {DonationId} in blockchain", pendingDonation.DonationId);
                return new BlockchainDonationSingleResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        private static decimal ParseAmount(string requestedAmount)
        {
            if (decimal.TryParse(requestedAmount, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsedAmount))
            {
                return parsedAmount;
            }
            return 0;
        }

        private async Task InvalidateDonationCache(string? eventCode = null)
        {
            try
            {
                await _cacheService.HashRemoveAsync("blockchain:donations", "all");
                await _cacheService.HashRemoveAsync("blockchain:donations", "total");

                if (!string.IsNullOrEmpty(eventCode))
                {
                    await _cacheService.HashRemoveAsync("blockchain:donations", $"event:{eventCode}");
                    await _cacheService.HashRemoveAsync("blockchain:donations", $"event:{eventCode}:total");
                }

                _logger.LogInformation("Successfully invalidated donation cache");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to invalidate donation cache, but continuing operation");
            }
        }

        public async Task<BlockchainDonationSingleResponse> GetDonationAsync(string id)
        {
            try
            {
                var cacheField = $"donation:{id}";
                
                var isExist = await _cacheService.IsExistHashAsync("blockchain:donations", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved donation {DonationId} from cache", id);
                }

                _logger.LogInformation("Fetching donation {DonationId} from API", id);
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/donations/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainDonationSingleResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainDonationSingleResponse { Success = false, Message = "Failed to deserialize response" };

                if (result.Success && result.Data != null)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:donations", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached donation {DonationId} for {Minutes} minutes", id, _cacheExpiration.TotalMinutes);
                }

                return result;
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
                var cacheField = "all";
                
                var isExist = await _cacheService.IsExistHashAsync("blockchain:donations", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved all donations from cache");
                }

                _logger.LogInformation("Fetching all donations from API");
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/donations");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainDonationListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainDonationListResponse { Success = false, Message = "Failed to deserialize response" };

               
                if (result.Success && result.Data != null)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:donations", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached all donations for {Minutes} minutes", _cacheExpiration.TotalMinutes);
                }

                return result;
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
                var cacheField = "total";
                
                var isExist = await _cacheService.IsExistHashAsync("blockchain:donations", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved total donations from cache");
                }

                _logger.LogInformation("Fetching total donations from API");
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/donations/total");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainTotalDonationResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainTotalDonationResponse { Success = false, Message = "Failed to deserialize response" };

               
                if (result.Success)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:donations", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached total donations for {Minutes} minutes", _cacheExpiration.TotalMinutes);
                }

                return result;
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
                var cacheField = $"event:{eventCode}";
                
                var isExist = await _cacheService.IsExistHashAsync("blockchain:donations", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved donations for event {EventCode} from cache", eventCode);
                }

                _logger.LogInformation("Fetching donations for event {EventCode} from API", eventCode);
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/donations/event/{eventCode}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainDonationListResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainDonationListResponse { Success = false, Message = "Failed to deserialize response" };

               
                if (result.Success && result.Data != null)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:donations", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached donations for event {EventCode} for {Minutes} minutes", eventCode, _cacheExpiration.TotalMinutes);
                }

                return result;
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
                var cacheField = $"event:{eventCode}:total";
                
                var isExist = await _cacheService.IsExistHashAsync("blockchain:donations", cacheField);
                if (isExist)
                {
                    _logger.LogInformation("Retrieved total donations for event {EventCode} from cache", eventCode);
                }

                _logger.LogInformation("Fetching total donations for event {EventCode} from API", eventCode);
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/donations/event/{eventCode}/total");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BlockchainTotalDonationResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new BlockchainTotalDonationResponse { Success = false, Message = "Failed to deserialize response" };

               
                if (result.Success)
                {
                    var serializedData = JsonSerializer.Serialize(result);
                    await _cacheService.HashSetAsync("blockchain:donations", cacheField, serializedData, _cacheExpiration);
                    _logger.LogInformation("Cached total donations for event {EventCode} for {Minutes} minutes", eventCode, _cacheExpiration.TotalMinutes);
                }

                return result;
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
