namespace Sneakers.Shop.Backend.Domain.Common
{
    public record Error(string Code, string Message)
    {
        public static Error NotFound(string message) => new("NotFound", message);
        public static Error Validation(string message) => new("Validation", message);
        public static Error Unauthorized(string message) => new("Unauthorized", message);
        public static Error Internal(string message) => new("Internal", message);
        public static Error Conflict(string message) => new("Conflict", message);
        public static Error BadRequest(string message) => new("BadRequest", message);
        public static Error Forbidden(string message) => new("Forbidden", message);
        public static Error TooManyRequests(string message) => new("TooManyRequests", message);
    }
}
