using LS.Application.Commands;
using LS.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace LS.Application.Handlers
{
    public class EarnPointsCommandHandler : BaseQueryHandler<EarnPointsCommand, bool>
    {
        private readonly IUserPointService _userPointService;
        private readonly ICache _cache;

        public EarnPointsCommandHandler(
            IUserPointService userPointService, 
            ICache cache,
            ILogger<EarnPointsCommandHandler> logger)
                : base(logger)
        {
            _userPointService = userPointService;
            _cache = cache;
        }

        protected override async Task<bool> ExecuteAsync(EarnPointsCommand request, CancellationToken cancellationToken)
        {
            var success = await _userPointService.EarnPoints(request.UserId, request.Points, cancellationToken);

            if (success)
            {
                var totalPoints = await _userPointService.GetUserTotalPointsAsync(request.UserId, cancellationToken);
                await _cache.SetValueAsync($"points_{request.UserId}", totalPoints.ToString());
            }

            return success;
        }
    }
}
