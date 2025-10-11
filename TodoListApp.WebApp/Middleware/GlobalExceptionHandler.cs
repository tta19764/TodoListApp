using System.Net;
using TodoListApp.WebApp.Models.Errors;

namespace TodoListApp.WebApp.Middleware;

/// <summary>
/// Global exception handler middleware that catches unhandled exceptions and redirects to error page.
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private static readonly Action<ILogger, string, string, string, string, Exception> LogUnhandledException =
        LoggerMessage.Define<string, string, string, string>(
            LogLevel.Error,
            new EventId(1, nameof(LogUnhandledException)),
            "Unhandled exception occurred. RequestId: {RequestId}, Path: {Path}, Method: {Method}, User: {User}");

    private readonly RequestDelegate next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next delegate.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">Thrown when next or logger is null.</exception>
    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        this.next = next ?? throw new ArgumentNullException(nameof(next));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Invokes the middleware to handle exceptions.
    /// </summary>
    /// <param name="httpContext">The http context.</param>
    /// <param name="environment">The host environment.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task InvokeAsync(HttpContext httpContext, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(environment);

        return this.InvokeInternalAsync(httpContext, environment);
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException => (int)HttpStatusCode.BadRequest,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            NotImplementedException => (int)HttpStatusCode.NotImplemented,
            InvalidOperationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };
    }

    private static string GetErrorCode(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException => "ERR_NULL_ARGUMENT",
            UnauthorizedAccessException => "ERR_UNAUTHORIZED",
            NotImplementedException => "ERR_NOT_IMPLEMENTED",
            InvalidOperationException => "ERR_INVALID_OPERATION",
            _ => "ERR_INTERNAL_SERVER"
        };
    }

    private static string GetErrorMessage(Exception exception, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            return exception.Message;
        }

        return exception switch
        {
            ArgumentNullException => "Invalid request data received.",
            UnauthorizedAccessException => "You don't have permission to access this resource.",
            NotImplementedException => "This feature is not yet available.",
            InvalidOperationException => "The requested operation could not be completed.",
            _ => "An unexpected error occurred. Our team has been notified and is working on it."
        };
    }

    private async Task InvokeInternalAsync(HttpContext httpContext, IHostEnvironment environment)
    {
        try
        {
            await this.next(httpContext);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception) when (exception is not StackOverflowException
                                          && exception is not OutOfMemoryException
                                          && exception is not ThreadAbortException)
        {
            await this.HandleExceptionAsync(httpContext, exception, environment);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext httpContext,
        Exception exception,
        IHostEnvironment environment)
    {
        var requestId = httpContext.TraceIdentifier;

        // Log the exception with full details
        LogUnhandledException(
            this.logger,
            requestId,
            httpContext.Request.Path,
            httpContext.Request.Method,
            httpContext.User?.Identity?.Name ?? "Anonymous",
            exception);

        // Determine error details based on environment
        var errorViewModel = new ErrorViewModel
        {
            RequestId = requestId,
            ErrorCode = GetErrorCode(exception),
            ErrorMessage = GetErrorMessage(exception, environment),
            ErrorDetails = environment.IsDevelopment() ? exception.ToString() : null,
            StatusCode = GetStatusCode(exception),
            Timestamp = DateTime.UtcNow,
        };

        // Set response status code
        httpContext.Response.StatusCode = errorViewModel.StatusCode;

        // Store error details in HttpContext.Items for the error page
        httpContext.Items["ErrorViewModel"] = errorViewModel;

        // Redirect to error page
        httpContext.Response.Redirect("/Home/Error");
        await Task.CompletedTask;
    }
}
