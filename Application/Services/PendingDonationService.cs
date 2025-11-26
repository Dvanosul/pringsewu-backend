using System.Text.Json;
using Sindika.AspNet.app015.Application.DTOs.Midtrans;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Common.Interfaces;
using StackExchange.Redis;

namespace Sindika.AspNet.app015.Application.Services
{
    public class PendingDonationService : IPendingDonationService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<PendingDonationService> _logger;
        private const string KeyPrefix = "pending:donation:";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromHours(24);

        public PendingDonationService(IConnectionMultiplexer redis, ILogger<PendingDonationService> logger)
        {
            _redis = redis;
            _logger = logger;
        }

        public async Task StorePendingDonationAsync(string orderId, PendingDonationDTO donation)
        {
            try
            {
                var db = _redis.GetDatabase();
                var key = $"{KeyPrefix}{orderId}";
                var serialized = JsonSerializer.Serialize(donation);
                await db.StringSetAsync(key, serialized, _cacheExpiration);
                _logger.LogInformation("Stored pending donation for order {OrderId}", orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to store pending donation for order {OrderId}", orderId);
                throw;
            }
        }

        public async Task<PendingDonationDTO?> GetPendingDonationAsync(string orderId)
        {
            try
            {
                var db = _redis.GetDatabase();
                var key = $"{KeyPrefix}{orderId}";
                var cached = await db.StringGetAsync(key);
                
                if (cached.IsNullOrEmpty)
                {
                    _logger.LogWarning("Pending donation not found for order {OrderId}", orderId);
                    return null;
                }

                var donation = JsonSerializer.Deserialize<PendingDonationDTO>(cached!);
                _logger.LogInformation("Retrieved pending donation for order {OrderId}", orderId);
                return donation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get pending donation for order {OrderId}", orderId);
                return null;
            }
        }

        public async Task RemovePendingDonationAsync(string orderId)
        {
            try
            {
                var db = _redis.GetDatabase();
                var key = $"{KeyPrefix}{orderId}";
                await db.KeyDeleteAsync(key);
                _logger.LogInformation("Removed pending donation for order {OrderId}", orderId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to remove pending donation for order {OrderId}", orderId);
            }
        }
    }
}
