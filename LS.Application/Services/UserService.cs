using LS.Domain.Entities;
using LS.Domain.Interfaces.Repositories;
using LS.Domain.Interfaces.Services;

namespace LS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUserAsync(string name, CancellationToken cancellationToken)
        {
            var exists = await _userRepository.ExistsByNameAsync(name, cancellationToken);
            if (exists)
            {
                throw new InvalidOperationException($"User with name '{name}' already exists.");
            }

            var userModel = new User
            {
                Name = name,
                TotalPoints = 0
            };

            var user = await _userRepository.CreateAsync(userModel, cancellationToken);
            return user;
        }

        public async Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            return user;
        }
    }
}
