using LS.Application.Mappings;
using LS.Infrastructure.Mappings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LS.Application.DIConfiguration
{
    public static class AutoMappersRegistration
    {
        // Extension method to add AutoMapper profiles into the service collection.
        public static void AddAutoMappers(this IServiceCollection services, ILogger logger)
        {
            logger.LogInformation($"Registering AutoMappers");

            // Register the Application project mapping profiles.
            services.AddAutoMapper(typeof(ApplicationAutoMapperProfile).Assembly);

            // Register the Infrastructure project mapping profiles.
            services.AddAutoMapper(typeof(DBMappingProfile).Assembly);
        }
    }
}
