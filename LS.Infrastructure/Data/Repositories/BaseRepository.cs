namespace LS.Infrastructure.Data.Repositories
{
    public class BaseRepository : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private bool _disposed = false;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
