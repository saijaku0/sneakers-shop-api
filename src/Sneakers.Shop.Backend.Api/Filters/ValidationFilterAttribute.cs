using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Sneakers.Shop.Backend.Api.Filters
{
    /// <summary>
    /// An action filter attribute that enforces model validation for action method arguments before the action
    /// executes.
    /// </summary>
    /// <remarks>If any action argument fails validation, the action is not executed and a bad request
    /// response is returned. Use this attribute to apply consistent model validation across multiple actions or
    /// controllers.</remarks>
    public class ValidationFilterAttribute() : ActionFilterAttribute
    {
        /// <summary>
        /// Executes asynchronous logic before and after the action method is invoked, including model validation for
        /// action arguments.
        /// </summary>
        /// <remarks>If any action argument fails validation, the action is not executed and a bad request
        /// response is returned. This method can be used to enforce model validation consistently across
        /// actions.</remarks>
        /// <param name="context">The context for the executing action, containing information about the current HTTP request and action
        /// arguments.</param>
        /// <param name="next">The delegate to execute the next action filter or the action itself.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the context for the executed
        /// action.</returns>
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context, 
            ActionExecutionDelegate next)
        {
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();
            Debug.WriteLine($"Executing action: {controllerName}.{actionName}");

            foreach (var arg in context.ActionArguments.Values)
            {
                if (arg == null)
                {
                    continue;
                }

                var validatorType = typeof(IValidator<>).MakeGenericType(arg.GetType());
                var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;
                
                if (validator == null)
                {
                    continue;
                }

                var validationContext = new ValidationContext<object>(arg);
                var validationResult = await validator.ValidateAsync(validationContext);

                if (!validationResult.IsValid)
                {
                    Debug.WriteLine($"Model validation failed for action: {controllerName}.{actionName}");
                    context.Result = new BadRequestObjectResult(validationResult.Errors);
                    return; 
                }
            }

            ActionExecutedContext executedContext = await next();

            Debug.WriteLine($"Executed action: {controllerName}.{actionName} with result: {executedContext.Result?.GetType().Name}");
            if (executedContext.Exception != null)
                Debug.WriteLine($"Action {controllerName}.{actionName} threw an exception: {executedContext.Exception.Message}");
        }
    }
}
