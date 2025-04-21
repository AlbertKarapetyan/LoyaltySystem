using LS.Application.Exceptions;
using LS.Domain.Interfaces.Repositories;
using LS.Domain.Interfaces.Services;
using LS.Infrastructure.Data;

namespace LS.Application.Services
{
    public class UserPointService : IUserPointService
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IUserPointRepository _userPointRepository;
        private readonly ApplicationDbContext _dbContext;

        public UserPointService(
            IUserService userService,
            IUserRepository userRepository,
            IUserPointRepository userPointRepository,
            ApplicationDbContext dbContext)
        {
            _userService = userService;
            _userRepository = userRepository;
            _userPointRepository = userPointRepository;
            _dbContext = dbContext;
        }

        public async Task<bool> EarnPoints(int userId, int points, CancellationToken cancellationToken)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
                if (user == null)
                {
                    throw new UserNotFoundException(userId);
                }

                await _userRepository.UpdateTotalPointsAsync(userId, points);
                await _userPointRepository.AddPointsHistoryAsync(userId, points);
                
                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<int> GetUserTotalPointsAsync(int userId, CancellationToken cancellationToken)
        {
            var cacheKey = $"points_{userId}";

            var user = await _userService.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            return user.TotalPoints;
        }
    }
}
