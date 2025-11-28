using Sindika.AspNet.Common.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Sindika.AspNet.app015.Application.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
        {
            _redis = redis;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var db = _redis.GetDatabase();
                var value = await db.StringGetAsync(key);
                if (value.IsNullOrEmpty) return default;
                return JsonSerializer.Deserialize<T>(value!);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to get cache for key {Key}", key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var db = _redis.GetDatabase();
                var serialized = JsonSerializer.Serialize(value);
                await db.StringSetAsync(key, serialized, expiration);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to set cache for key {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                var db = _redis.GetDatabase();
                await db.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to remove cache for key {Key}", key);
            }
        }

        public async Task<bool> IsExistAsync(string key)
        {
            try
            {
                var db = _redis.GetDatabase();
                return await db.KeyExistsAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to check cache existence for key {Key}", key);
                return false;
            }
        }

        public async Task HashSetAsync(string key, string field, string value, TimeSpan? expiration = null)
        {
            try
            {
                var db = _redis.GetDatabase();
                await db.HashSetAsync(key, field, value);
                if (expiration.HasValue)
                {
                    await db.KeyExpireAsync(key, expiration);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to hash set cache for key {Key} field {Field}", key, field);
            }
        }

        public async Task<string?> HashGetAsync(string key, string field)
        {
            try
            {
                var db = _redis.GetDatabase();
                var value = await db.HashGetAsync(key, field);
                return value.IsNullOrEmpty ? null : value.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to hash get cache for key {Key} field {Field}", key, field);
                return null;
            }
        }

        public async Task HashRemoveAsync(string key, string field = "*")
        {
            try
            {
                var db = _redis.GetDatabase();
                if (field == "*")
                {
                    var fields = await db.HashKeysAsync(key);
                    if (fields.Length == 0)
                    {
                        return;
                    }

                    await db.HashDeleteAsync(key, fields);
                    return;
                }

                await db.HashDeleteAsync(key, field);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to hash remove cache for key {Key} field {Field}", key, field);
            }
        }

        public async Task<bool> IsExistHashAsync(string key, string field)
        {
            try
            {
                var db = _redis.GetDatabase();
                return await db.HashExistsAsync(key, field);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to check hash cache existence for key {Key} field {Field}", key, field);
                return false;
            }
        }

        public Task<Dictionary<string, string>> HashGetAllAsync(string key)
        {
            try
            {
                var db = _redis.GetDatabase();
                var entries = db.HashGetAll(key);
                return Task.FromResult(entries.ToDictionary(
                    e => e.Name.ToString(),
                    e => e.Value.ToString()
                ));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to hash get all for key {Key}", key);
                return Task.FromResult(new Dictionary<string, string>());
            }
        }

        public async Task HashSetBatchAsync(string key, Dictionary<string, string> entries, TimeSpan? expiration = null)
        {
            try
            {
                var db = _redis.GetDatabase();
                var batch = db.CreateBatch();
                var hashEntries = entries.Select(e => new HashEntry(e.Key, e.Value)).ToArray();
                var setTask = batch.HashSetAsync(key, hashEntries);
                Task<bool>? expireTask = null;
                if (expiration.HasValue)
                {
                    expireTask = batch.KeyExpireAsync(key, expiration);
                }

                batch.Execute();
                if (expireTask is null)
                {
                    await setTask;
                }
                else
                {
                    await Task.WhenAll(setTask, expireTask);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to hash set batch for key {Key}", key);
            }
        }
    }
}
