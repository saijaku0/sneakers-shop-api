namespace Sneakers.Shop.Backend.Api.Models
{
    /// <summary>
    /// Represents a standard error response returned by the API when a request fails.
    /// </summary>
    /// <param name="StatusCode">
    /// The HTTP status code associated with the error. Used by the client to determine the type of error.
    /// </param>
    /// <param name="ErrorCode">
    /// The error code that identifies the specific reason for the failure. Can be used for programmatic error handling.
    /// </param>
    /// <param name="ErrorMessage">
    /// A human-readable message describing the cause of the error. Intended for display to users or for logging.
    /// </param>
    /// <param name="TraceId">
    /// A unique trace identifier associated with the request. Used for tracking and debugging errors.
    /// </param>
    public record ErrorResponse(
        int StatusCode,
        string ErrorCode,
        string ErrorMessage,
        string TraceId);
}
