using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Homely.AspNetCore.Mvc.Helpers.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Homely.AspNetCore.Mvc.Helpers.Filters
{
    // http://www.jerriepelser.com/blog/validation-response-aspnet-core-webapi/
    public class ValidateModelFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            // Ru-roh - something bad has happened :(
            var apiErrors = context.ModelState.ToApiErrors();
            var errorViewModel = new ErrorViewModel(apiErrors);
            context.Result = new BadRequestObjectResult(errorViewModel);
        }
    }
}
