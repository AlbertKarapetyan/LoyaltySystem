using LS.Domain.Entities;

namespace LS.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(string name, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken);
    }
}
