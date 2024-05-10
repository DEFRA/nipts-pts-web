using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Defra.PTS.Web.Application.Features.Pipelines;

[ExcludeFromCodeCoverage]
public class LoggingPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingPipeline<TRequest, TResponse>> _logger;

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true
    };

    public LoggingPipeline(ILogger<LoggingPipeline<TRequest, TResponse>> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Log("Request: {request}", request);

        var response = await next();

        Log("Response: {response}", response);

        return response;
    }

    private void Log<T>(string message, T instance)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace(message, JsonSerializer.Serialize(instance, _serializerOptions));
        }
    }
}