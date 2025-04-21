using MediatR;
using Microsoft.Extensions.Logging;

namespace LS.Application.Handlers
{
    public abstract class BaseCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        protected BaseCommandHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await ExecuteAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error - Command {CommandName}: {ErrorMessage}", typeof(TRequest).Name, ex.Message);
                throw;
            }
        }

        protected abstract Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
    }
}
