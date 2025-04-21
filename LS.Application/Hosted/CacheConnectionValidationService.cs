

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace LS.Application.DIConfiguration.Hosted
{
    public class CacheConnectionValidationService : BackgroundService
    {
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly ILogger<CacheConnectionValidationService> _logger;

        public CacheConnectionValidationService(IConnectionMultiplexer  multiplexer, ILogger<CacheConnectionValidationService> logger)
        {
            this._multiplexer = multiplexer;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var pong = await _multiplexer.GetDatabase().PingAsync();
                _logger.LogInformation($"Redis connection verified successfully: Pong in {pong.TotalMilliseconds} ms");
            }
            
            catch
            {
                _logger.LogError($"Redis connection verification failed:");
            }

            await Task.CompletedTask;
        }
    }
}
