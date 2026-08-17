using Microsoft.AspNetCore.Builder;

namespace CashFlow.Infrastructure.Logging;

public static class LoggingExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder application)
        => application.UseMiddleware<CorrelationIdMiddleware>();
}