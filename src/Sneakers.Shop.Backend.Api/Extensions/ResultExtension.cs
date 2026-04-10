using Microsoft.AspNetCore.Mvc;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Api.Extensions
{
    /// <summary>
    /// Provides extension methods for converting operation results to HTTP action results in ASP.NET Core controllers.
    /// </summary>
    public static class ResultExtension
    {
        /// <summary>
        /// Converts a result instance to an appropriate IActionResult based on the result's success or error code.
        /// </summary>
        /// <remarks>Maps common error codes such as "NotFound", "ValidationError", "Unauthorized", and
        /// others to standard HTTP responses. If the error code is not recognized, a BadRequest response is
        /// returned.</remarks>
        /// <typeparam name="T">The type of the value contained in the result if the operation is successful.</typeparam>
        /// <param name="result">The result object representing the outcome of an operation, including success or error information.</param>
        /// <param name="controller">The controller instance used to generate the corresponding IActionResult.</param>
        /// <returns>An IActionResult representing the HTTP response corresponding to the result's status. Returns a success
        /// response with the value if the result is successful; otherwise, returns an error response based on the error
        /// code.</returns>
        public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
        {
            if (result.IsSuccess)
                return controller.Ok(result.Value);

            return result.Error!.Code switch
            {
                "NotFound" => controller.NotFound(result.Error.Message),
                "ValidationError" => controller.BadRequest(result.Error.Message),
                "Unauthorized" => controller.Unauthorized(result.Error.Message),
                "Internal" => controller.StatusCode(500, result.Error.Message),
                "Conflict" => controller.Conflict(result.Error.Message),
                "BadRequest" => controller.BadRequest(result.Error.Message),
                "Forbidden" => controller.Forbid(result.Error.Message),
                "TooManyRequests" => controller.StatusCode(429, result.Error.Message),
                _ => controller.BadRequest(result.Error.Message)
            };
        }
    }
}

