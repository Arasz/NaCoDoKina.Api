using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace NaCoDoKina.Api.ActionFilters
{
    /// <summary>
    /// Validates all action arguments that name ends with id and is of type long. Adds errors to
    /// model state if invalid. <remarks> Fluent validator is not created for primitive types
    /// validation. To remove validation from each action filter implementation was chosen </remarks>
    /// </summary>
    public class IdValidationActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var invalidIds = context.ActionArguments
                .Where(pair => pair.Key.ToLowerInvariant().EndsWith("id"))
                .Where(pair => pair.Value is long id && id <= 0)
                .Select(pair => (Name: pair.Key, Id: pair.Value));

            foreach (var valueTuple in invalidIds)
            {
                context.ModelState.AddModelError(valueTuple.Name, $"Invalid {valueTuple.Name} value {valueTuple.Id}");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}