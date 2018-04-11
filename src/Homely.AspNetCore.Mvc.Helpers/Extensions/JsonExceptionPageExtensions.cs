using FluentValidation;
using Homely.AspNetCore.Mvc.Helpers.Models;
using Homely.AspNetCore.Mvc.Helpers.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class JsonExceptionPageExtensions
    {
        private const string JsonContentType = "application/json";

        /// <summary>
        /// Adds a simple json error message for a WebApi/REST website.
        /// </summary>
        /// <param name="app">A class that provides the mechanisms to configure an application's request.</param>
        /// <param name="corsPolicyName">Optional: Name of the CORS policy to re-add to the specific details back to the response header.</param>
        /// <param name="includeStackTrace">Wether a stacktrace is included in the output or not.</param>
        /// <param name="customExceptionFunction">Optional: Custom function to handle any unique logic when an exception is handled. For example, what do you do if a custom exception is thrown? You can handle any custom code, here.</param>
        /// <returns>This class that provides the mechanisms to configure an application's request</returns>
        public static IApplicationBuilder UseJsonExceptionPages(this IApplicationBuilder app,
                                                                string corsPolicyName = null,
                                                                bool includeStackTrace = false,
                                                                Func<Exception, JsonExceptionPageResult> customExceptionFunction = null)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            
            return app.UseExceptionHandler(configure => configure.Run(
                                                            async httpContext => await ExceptionResponseAsync(httpContext, 
                                                            corsPolicyName,
                                                            includeStackTrace,
                                                            customExceptionFunction)));
        }

        private static async Task ExceptionResponseAsync(HttpContext httpContext,
                                                         string corsPolicyName = null,
                                                         bool includeStackTrace = false,
                                                         Func<Exception, JsonExceptionPageResult> customExceptionFunction = null)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            
            if (!string.IsNullOrWhiteSpace(corsPolicyName))
            {
                await ReApplyCorsPolicyToHeaderAsync(httpContext, corsPolicyName).ConfigureAwait(false);
            }

            var exceptionFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>() ?? new ExceptionHandlerFeature
            {
                Error = new Exception("An unhandled and unexpected error has occured. Ro-roh :~(.")
            };
            
            var exception = exceptionFeature.Error;
            var statusCode = exception is ValidationException 
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.InternalServerError;
            IEnumerable<ApiError> apiErrors = new List<ApiError>();

            // It's possible the developer wishes to do some custom error handling which
            // results in some json exception result. So, we now run this custom code if it's provided.
            JsonExceptionPageResult jsonExceptionPageResult = null;
            if (customExceptionFunction != null)
            {
                jsonExceptionPageResult = customExceptionFunction(exception);
            }
            
            // It's possible that result contains no result/nothing passed
            // -or-
            // no custom exception logic was provided/executed.
            if (jsonExceptionPageResult != null)
            {
                statusCode = jsonExceptionPageResult.StatusCode;
                apiErrors = jsonExceptionPageResult.ApiErrors;
            }
            else
            {
                // Final fallback - any/all other errors.
                apiErrors = new [] { new ApiError(exception.Message)};
            }
            
            var errorModel = new ErrorViewModel(apiErrors);

            // Prepare the actual RESPONSE payload.
            httpContext.Response.StatusCode = (int) statusCode;
            httpContext.Response.ContentType = JsonContentType;
            
            await httpContext.Response.WriteApiErrorsAsJsonAsync(apiErrors,
                                                                 includeStackTrace 
                                                                     ? exception.StackTrace
                                                                     : null);
        }

        /// The headers are cleaered when a custom exception is applied. 
        /// REF: https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNetCore.Http.Extensions/ResponseExtensions.cs#L19
        ///      https://stackoverflow.com/questions/47225012/how-to-send-an-http-4xx-5xx-response-with-cors-headers-in-an-aspnet-core-web-app/47232876?noredirect=1#comment81470664_47232876
        /// It's a PITA :( As such, we need to re-apply the named CORS policy to the response headers.
        private static async Task ReApplyCorsPolicyToHeaderAsync(HttpContext httpContext,
                                                                 string corsPolicyName)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (string.IsNullOrWhiteSpace(corsPolicyName))
            {
                throw new ArgumentException(nameof(corsPolicyName));
            }

            // REF: https://stackoverflow.com/questions/47225012/how-to-send-an-http-4xx-5xx-response-with-cors-headers-in-an-aspnet-core-web-app/47232876
            if (!(httpContext.RequestServices.GetService(typeof(ICorsService)) is ICorsService corsService))
            {
                return;
            }

            if (!(httpContext.RequestServices.GetService(typeof(ICorsPolicyProvider)) is ICorsPolicyProvider corsPolicyProvider))
            {
                // We've failed to retrieve the named CORS policy and as such, can't add any headers back to the response :(
                return;
            }

            var corsPolicy = await corsPolicyProvider.GetPolicyAsync(httpContext, corsPolicyName);
            corsService.ApplyResult(corsService.EvaluatePolicy(httpContext, corsPolicy),
                                    httpContext.Response);
        }
    }
}
