using LS.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace LS.Infrastructure.Services
{
    public class RedisService : ICache
    {
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly ILogger<RedisService> _logger;

        public RedisService(IConnectionMultiplexer multiplexer, ILogger<RedisService> logger)
        {
            _multiplexer = multiplexer;
            _logger = logger;
        }

        public async Task<string?> GetValueAsync(string key)
        {
            try
            {
                var db = _multiplexer.GetDatabase();
                var redisValue = await db.StringGetAsync(key);

                if (redisValue.IsNullOrEmpty)
                {
                    _logger.LogInformation("Key '{Key}' not found or is empty in Redis.", key);
                    return null;
                }

                _logger.LogInformation("Successfully retrieved value for key: {Key}", key);
                return redisValue;
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogError(ex, "Failed to connect to Redis while getting value for key: {Key}.", key);
                throw new Exception("Failed to connect to Redis.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to get value for key: {Key}", key);
                throw;
            }
        }

        public async Task SetValueAsync(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Value cannot be null.");

            try
            {
                _logger.LogInformation("Attempting to set value in Redis for key: {Key}", key);
                var db = _multiplexer.GetDatabase();

                bool isSet = await db.StringSetAsync(key, value);

                if (isSet)
                {
                    _logger.LogInformation("Successfully set value in Redis for key: {Key}.", key);
                }
                else
                {
                    _logger.LogWarning("Failed to set value in Redis for key: {Key}.", key);
                }
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogError(ex, "Failed to connect to Redis while setting value for key: {Key}.", key);
                throw new Exception("Failed to connect to Redis.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while setting value for key: {Key}.", key);
                throw;
            }
        }
    }
}