using Homely.AspNetCore.Mvc.Helpers.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

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
    }
}
