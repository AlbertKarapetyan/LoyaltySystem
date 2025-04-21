using LS.Domain.Entities;

namespace LS.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<User> CreateAsync(User user, CancellationToken cancellationToken);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
        Task UpdateTotalPointsAsync(int id, int points);
    }
}
