using LS.Application.Services;
using LS.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LS.Application.DIConfiguration
{
    public static class DIServicesRegistration
    {
        // Extension method to add domain services.
        public static void AddServices(this IServiceCollection services)
        {
            // Register IUserService implementation.
            services.AddScoped<IUserService, UserService>();

            // Register IUserPointService implementation.
            services.AddScoped<IUserPointService, UserPointService>();
        }
    }
}
