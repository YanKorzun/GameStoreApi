using System;
using System.Linq;
using System.Text.RegularExpressions;
using GameStore.DAL.Enums;
using GameStore.WEB.Constants;
using GameStore.WEB.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameStore.WEB.Filters.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.QueryString.HasValue)
            {
                return;
            }

            var genre = GetValueFromQuery(context, nameof(ProductParameters.Genre));

            var name = GetValueFromQuery(context, nameof(ProductParameters.Name));

            var parseResult = int.TryParse(GetValueFromQuery(context, nameof(ProductParameters.AgeRating)),
                out var ageRating);

            var parameters = new ProductParameters();

            if (string.IsNullOrWhiteSpace(parameters.Genre))
            {
            }
            else if (!Regex.IsMatch(genre, RegexConstants.OnlyAlphabeticChars))
            {
                context.Result = new BadRequestObjectResult("Wrong genre format");
            }

            if (ageRating > Enum.GetNames(typeof(AgeProductRating)).Length)
            {
                context.Result = new BadRequestObjectResult("Wrong age rating format");
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