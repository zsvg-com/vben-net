using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Vben.Common.Core.Token;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken token)
    {
        _logger.LogError(exception, "An unhandled exception occurred.");

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server Error",
            Type = "https://httpstatuses.com/500"
        };
        await context.Response.WriteAsJsonAsync(problemDetails, token);
        return true; // 表示异常已处理
    }
}