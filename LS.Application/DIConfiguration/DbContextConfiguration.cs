using LS.Domain.Interfaces.Repositories;
using LS.Infrastructure.Data;
using LS.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LS.Application.DIConfiguration
{
    public static class DbContextConfiguration
    {
        // Extension method to configure the application's database context.
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            // Check if DBProvider is null or empty
            if (string.IsNullOrEmpty(configuration["DBProvider"]))
            {
                logger.LogError("DBProvider is required in the configuration.");
                throw new ArgumentException("DBProvider is required in the configuration.");
            }

            logger.LogInformation($"Configuring database provider: {configuration["DBProvider"]}");

            // Checking ConnectionString
            var connectionString = configuration.GetConnectionString(configuration["DBProvider"]!);
            if (string.IsNullOrEmpty(connectionString))
            {
                logger.LogError($"ConnectionString is required for the provider: {configuration["DBProvider"]}");
                throw new ArgumentException($"ConnectionString is required for the provider: {configuration["DBProvider"]}");
            }

            // Setup based on the specified DBProvider.
            switch (configuration["DBProvider"])
            {
                case "PGSql":
                    {
                        // Configure PostgreSQL database
                        logger.LogInformation("Configuring PostgreSQL database.");

                        services.AddDbContext<ApplicationDbContext>(
                        options => options.UseNpgsql(
                            connectionString,
                            x => x.MigrationsAssembly("LS.Data.Migrations")
                        ));

                        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

                        break;
                    }
                default:
                    {
                        // Log and throw exception for unsupported providers
                        logger.LogError($"Unsupported database provider: {configuration["DBProvider"]}");
                        throw new Exception($"Unsupported provider: {configuration["DBProvider"]}");
                    }
            }

            // Register DbContext and repositories into DI container.
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserPointRepository, UserPointRepository>();
        }
    }
}
