using LS.Application.DIConfiguration.Hosted;
using LS.Domain.Interfaces.Services;
using LS.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace LS.Application.DIConfiguration
{
    public static class CacheConfiguration
    {
        /// <summary>
        /// Configures Redis cache and registers it in the DI container.
        /// </summary>
        /// <param name="services">Service collection.</param>
        public static void AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddSingleton<IConnectionMultiplexer>(provider =>
            {
                var redisConnection = configuration["Redis:ConnectionString"];
                if (string.IsNullOrEmpty(redisConnection))
                {
                    throw new ArgumentException("Redis:ConnectionString configuration is required.");
                }

                var logger = provider.GetService<ILogger<CacheConnectionValidationService>>();
                try
                {
                    logger?.LogInformation("Connecting to Redis...");

                    var multiplexer = ConnectionMultiplexer.Connect(redisConnection);
                    if (multiplexer.IsConnected)
                    {
                        logger?.LogInformation("Successfully connected to Redis.");
                    }
                    else
                    {
                        logger?.LogWarning("Failed to connect to Redis.");
                    }

                    return multiplexer;
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "Failed to connect to Redis.");

                    throw;
                }
            });

            // Register RedisService as ICache abstraction.
            services.AddSingleton<ICache, RedisService>();

        }
    }
}
