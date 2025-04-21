using AutoMapper;
using LS.Domain.Entities;
using LS.Domain.Interfaces.Repositories;
using LS.Infrastructure.Data.Models;

namespace LS.Infrastructure.Data.Repositories
{
    public class UserPointRepository : BaseRepository, IUserPointRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserPointRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserPoint> AddPointsHistoryAsync(int userId, int points)
        {
            var userPointModel = new UserPointModel
            {
                UserId = userId,
                Points = points,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.UserPoints.AddAsync(userPointModel);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserPoint>(userPointModel);
        }
    }
}
