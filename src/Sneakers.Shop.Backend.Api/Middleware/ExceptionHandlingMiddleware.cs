using Microsoft.AspNetCore.Diagnostics;
using Sneakers.Shop.Backend.Api.Models;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Api.Middleware
{
    /// <summary>
    /// Provides middleware for handling exceptions by generating standardized JSON error responses for HTTP requests.
    /// </summary>
    /// <remarks>This middleware maps specific exception types to corresponding HTTP status codes and error
    /// codes. Domain exceptions result in a 400 Bad Request, unauthorized access results in a 401 Unauthorized, and all
    /// other exceptions result in a 500 Internal Server Error. Each error response is formatted as JSON and includes a
    /// trace identifier to facilitate request correlation and troubleshooting.</remarks>
    /// <param name="logger">The logger used to record exception details and diagnostic information.</param>
    public class ExceptionHandlingMiddleware(
        ILogger<ExceptionHandlingMiddleware> logger)
     : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
        
        /// <summary>
        /// Attempts to handle the specified exception by writing an appropriate JSON error response to the HTTP context
        /// asynchronously.
        /// </summary>
        /// <remarks>The response status code and error code are determined by the type of exception.
        /// Domain exceptions result in a 400 Bad Request, unauthorized access results in a 401 Unauthorized, and all
        /// other exceptions result in a 500 Internal Server Error. The response is formatted as JSON and includes a
        /// trace identifier for correlation.</remarks>
        /// <param name="context">The HTTP context for the current request. Used to write the error response.</param>
        /// <param name="exception">The exception to handle. Determines the error response details based on its type.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The result is <see langword="true"/> if the exception was
        /// handled and a response was written; otherwise, <see langword="false"/>.</returns>
        public async ValueTask<bool> TryHandleAsync(
            HttpContext context, 
            Exception exception,
            CancellationToken ct)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            var (statusCode, errorCode) = exception switch
            {
                DomainException domainEx => (StatusCodes.Status400BadRequest, "DOMAIN_ERROR"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "UNAUTHORIZED"),
                _ => (StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR")
            };

            var errorResponse = new ErrorResponse(
                StatusCode: statusCode,
                ErrorCode: errorCode,
                ErrorMessage: exception.Message,
                TraceId: context.TraceIdentifier
            );

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(errorResponse, ct);
            return true;
        }
    }
}
