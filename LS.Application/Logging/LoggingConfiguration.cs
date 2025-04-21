using Microsoft.AspNetCore.Builder;
using Serilog;

namespace LS.Application.Logging
{
    public static class LoggingConfiguration
    {
        public static void ConfigureSerilog(WebApplicationBuilder builder)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Async(a => a.Console())
                    .WriteTo.Async(a => a.File("logs/log-.txt", rollingInterval: RollingInterval.Day))
                    .CreateLogger();

                builder.Host.UseSerilog();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with the Serilog's settings: " + ex.Message);
                throw;
            }
        }
    }
}
