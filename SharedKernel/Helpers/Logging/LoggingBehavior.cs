using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Helpers.Logging;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var logObject = request.DeepCloneWithLoggingFilters();

        if (request.GetType().ToString().EndsWith("Command"))
        {
            _logger.LogInformation("----- Sending command: {CommandName} - ({@Command}) -----", request.GetType().Name, logObject);
            var response = await next();
            _logger.LogInformation("----- Received response for command: {CommandName} -----", request.GetType().Name);

            return response;
        }

        _logger.LogInformation("----- Sending query: {QueryName} - ({@Query}) -----", request.GetType().Name, logObject);
        var result = await next();
        _logger.LogInformation("----- Received result for query: {QueryName} -----", request.GetType().Name);

        return result;
    }
}