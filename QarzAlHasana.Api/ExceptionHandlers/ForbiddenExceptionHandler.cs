using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Api.ExceptionHandlers;

public sealed class ForbiddenExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ForbiddenException forbiddenException)
            return false;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Dastresi mojaz nist",
            Detail = forbiddenException.Message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}