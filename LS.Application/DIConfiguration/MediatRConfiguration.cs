using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LS.Application.DIConfiguration
{
    public static class MediatRConfiguration
    {
        public static void AddMediatr(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });
        }
    }
}
