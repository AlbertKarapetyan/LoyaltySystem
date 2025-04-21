using LS.Application.Queries;
using LS.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace LS.Application.Handlers
{
    internal class GetUserTotalPointsQueryHandler : BaseQueryHandler<GetUserTotalPointsQuery, int?>
    {
        private readonly IUserPointService _userPointService;
        private readonly ICache _cache;
        private readonly ILogger<GetUserTotalPointsQueryHandler> _logger;

        public GetUserTotalPointsQueryHandler(
            IUserPointService userPointService,
            ICache cache,
            ILogger<GetUserTotalPointsQueryHandler> logger)
            : base(logger)
        {
            _userPointService = userPointService;
            _cache = cache;
            _logger = logger;
        }

        protected override async Task<int?> ExecuteAsync(GetUserTotalPointsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"points_{request.UserId}";

            var cachedValue = await _cache.GetValueAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedValue) && int.TryParse(cachedValue, out var cachedPoints))
            {
                _logger.LogInformation("Cache hit for user {UserId}", request.UserId);
                return cachedPoints;
            }

            _logger.LogInformation("Cache miss for user {UserId}", request.UserId);

            var points = await _userPointService.GetUserTotalPointsAsync(request.UserId, cancellationToken);
            
            await _cache.SetValueAsync(cacheKey, points.ToString());

            return points;
        }
    }
}
