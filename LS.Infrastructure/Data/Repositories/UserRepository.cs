using AutoMapper;
using LS.Domain.Entities;
using LS.Domain.Interfaces.Repositories;
using LS.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LS.Infrastructure.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
        {
            var userModel = new UserModel
            {
                Name = user.Name,
                TotalPoints = user.TotalPoints
            };

            await _dbContext.Users.AddAsync(userModel);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<User>(userModel);
        }

        public async Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            var userDbModel = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            return _mapper.Map<User>(userDbModel);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Name == name, cancellationToken);
        }

        public async Task UpdateTotalPointsAsync(int userId, int points)
        {
            var affected = await _dbContext.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.TotalPoints, u => u.TotalPoints + points)
                );
        }
    }
}
