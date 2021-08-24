using System;
using System.Linq;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Parameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameStore.WEB.Filters.ActionFilters
{
    public class ProductFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.QueryString.HasValue)
            {
                return;
            }

            var orderProp = GetValueFromQuery(context, nameof(ProductParametersDto.OrderBy));
            if (!string.IsNullOrWhiteSpace(orderProp))
            {
                var props = typeof(Product).GetProperties().ToList()
                    .Where(o => string.Equals(o.Name, orderProp, StringComparison.CurrentCultureIgnoreCase));

                if (!props.Any())
                {
                    context.Result = new BadRequestObjectResult("Wrong order format");
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private static string GetValueFromQuery(ActionContext context, string parameterName)
        {
            context.HttpContext.Request.Query.TryGetValue(parameterName, out var testName);
            return testName.FirstOrDefault();
        }
    }
}