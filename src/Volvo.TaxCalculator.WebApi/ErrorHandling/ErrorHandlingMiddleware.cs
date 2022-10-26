using FluentValidation;
using Volvo.TaxCalculator.Utils;

namespace Volvo.TaxCalculator.WebApi.ErrorHandling;

public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger("ErrorHandling");
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (HttpStatusException e)
        {
            await HandleExceptionAsync(context, e, (int)e.Status, e.Message);
        }
        catch (ValidationException e)
        {
            await HandleExceptionAsync(context, e, 400, e.Errors.Select(e => e.ErrorMessage).ToArray());
        }
        catch (BadHttpRequestException e)
        {
            await HandleExceptionAsync(context, e, 400, "Invalid vehicle passed");
        }
        catch (ArgumentOutOfRangeException e)
        {
            await HandleExceptionAsync(context, e, 422, e.Message);
        }
        catch (AggregateException e)
        {
            await HandleExceptionAsync(context, e, 500, e.InnerExceptions.Select(i => i.Message).ToArray());
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e, 500, e.Message);
        }
    }

    private Task WriteErrorResponseAsync(HttpContext context, int code, IEnumerable<string> errors)
    {
        var response = new ErrorResult(errors, context.TraceIdentifier);
        context.Response.StatusCode = code;
        return context.Response.WriteAsJsonAsync(response);
    }

    private Task HandleExceptionAsync(HttpContext context, Exception e, int code, params string[] errors)
    {
        _logger.LogError(e, "Error occured");

        if (!context.Response.HasStarted)
        {
            var responseHeaders = new HeaderDictionary();
            foreach (var header in context.Response.Headers)
                responseHeaders.Add(header);

            context.Response.Clear();

            foreach (var header in responseHeaders)
                context.Response.Headers.Add(header);
        }

        return WriteErrorResponseAsync(context, code, errors);
    }
}