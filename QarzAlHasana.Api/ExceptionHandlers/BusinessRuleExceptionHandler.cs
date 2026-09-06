using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QarzAlHasana.Domain.Exceptions;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Api.ExceptionHandlers;

public sealed class BusinessRuleExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not BusinessRuleException businessRuleException)
            return false;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Ghanoon-e kasb-o-kar naghz shode",
            Detail = businessRuleException.Message,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["code"] = businessRuleException.Code;

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}