using LS.Domain.Entities;

namespace LS.Domain.Interfaces.Repositories
{
    public interface IUserPointRepository
    {
        Task<UserPoint> AddPointsHistoryAsync(int userId, int points);
    }
}
