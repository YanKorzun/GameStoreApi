using System.Linq;
using GameStore.WEB.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameStore.WEB.Filters.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var parameters = context.ActionArguments.SingleOrDefault(p => p.Value is QueryStringParameters).Value;
            if (parameters is not QueryStringParameters { PageSize: < 1 } queryString) return;
            context.Result = new BadRequestObjectResult("Object is null");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}