using LS.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LS.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IDisposable
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserPointModel> UserPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
