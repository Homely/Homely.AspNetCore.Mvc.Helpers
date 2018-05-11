using Homely.AspNetCore.Mvc.Helpers.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// All Controllers require 'default' Authorization.
        /// </summary>
        /// <param name="options">Application Mvc specific options.</param>
        /// <returns>The Mvc options which were updated.</returns>
        /// <remarks>1) Make sure you've applied <code>.AddAuthorization()</code>. 2) You can manually target specific Controllers or ActionMethods to *not* require Authorization by using the <code>[AllowAnonymous]</code> attrbiute.</remarks>
        public static MvcOptions WithGlobalAuthorization(this MvcOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.Filters.Add(new AuthorizeFilter(policy));

            return options;
        }

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
