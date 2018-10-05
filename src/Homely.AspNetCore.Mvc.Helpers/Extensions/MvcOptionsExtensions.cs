using Homely.AspNetCore.Mvc.Helpers.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// Use a CancellationToken in your ASP.NET Core action method to stop execution when a user cancels a request from their browser.
        /// </summary>
        /// <param name="options">Application Mvc specific options.</param>
        /// <returns>The Mvc options which were updated.</returns>
        /// <remarks>Detailed Info: https://andrewlock.net/using-cancellationtokens-in-asp-net-core-mvc-controllers/</remarks>
        public static MvcOptions WithGlobalCancelledRequestHandler(this MvcOptions options)
        {
            options.Filters.Add<OperationCancelledExceptionFilter>();

            return options;
        }

        /// <summary>
        /// Circuit break the request early on, if there's a bad model.
        /// </summary>
        /// <param name="options">Application Mvc specific options.</param>
        /// <returns>The Mvc options which were updated.</returns>
        /// <remarks>If the requested data model is bad, this will return an HTTP 400 BAD REQUEST with the error projected into the common API Error model.</remarks>
        public static MvcOptions WithEarlyWaningModelValidation(this MvcOptions options)
        {
            options.Filters.Add<ValidateModelFilter>();

            return options;
        }
    }
}
