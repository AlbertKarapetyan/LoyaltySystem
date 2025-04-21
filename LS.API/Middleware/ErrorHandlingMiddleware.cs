using FluentValidation;
using LS.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LS.API.Middleware
{
    // Middleware for centralized error handling and logging.
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        // Constructor injecting the next request delegate and logger.
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Middleware invocation handling exceptions during request processing.
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UserNotFoundException ex)
            {
                // Specific handling for user not found.
                _logger.LogWarning(ex.Message);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (ValidationException ex)
            {
                // Handling FluentValidation exceptions.
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    errors = ex.Errors.GroupBy(e => e.PropertyName)
                                      .ToDictionary(
                                          g => g.Key,
                                          g => g.Select(e => e.ErrorMessage).ToArray()
                                      ),
                    status = 400
                });
            }
            catch (InvalidOperationException ex)
            {
                // Handling invalid operations separately.
                _logger.LogWarning(ex.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Generic handler for unhandled exceptions.
                _logger.LogError(ex, "Unhandled exception");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = "Internal server error" });
            }
        }
    }
}
