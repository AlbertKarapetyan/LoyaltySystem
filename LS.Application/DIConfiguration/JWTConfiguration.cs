using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LS.Application.DIConfiguration
{
    public static class JWTConfiguration
    {
        // Extension method to add and configure JWT Authentication.
        public static void AddJWTAuthentication(this IServiceCollection services, ILogger logger, IConfiguration configuration)
        {
            // Add JWT Bearer Authentication.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),

                    RequireSignedTokens = false,

                    ClockSkew = TimeSpan.Zero  // No tolerance for token expiration.

                };

                // Configure JWT Bearer Events for logging authentication events.
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        logger.LogWarning("Authentication failed: {Error}", context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        logger.LogInformation("Token validated successfully.");
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
