namespace LS.Domain.Interfaces.Services
{
    public interface IUserPointService
    {
        Task<bool> EarnPoints(int userId, int points, CancellationToken cancellationToken);
        Task<int> GetUserTotalPointsAsync(int userId, CancellationToken cancellationToken);
    }
}
