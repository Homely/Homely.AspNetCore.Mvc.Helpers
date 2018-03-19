using FluentValidation;
using Homely.AspNetCore.Mvc.Helpers.Helpers;
using Homely.AspNetCore.Mvc.Helpers.Models;
using Homely.AspNetCore.Mvc.Helpers.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class JsonExceptionPageExtensions
    {
        private const string JsonContentType = "application/json";
        private static ConcurrentDictionary<int, string> staticCodeDictionary = new ConcurrentDictionary<int, string>();

        /// <summary>
        /// Handles all status code results as json results. For HTTP 500 errors, has it's own logic to generate a smarter error response.
        /// </summary>
        /// <remarks>All responses, regardless of the status code, conform to the standard ApiErrorResponse schema.</remarks>
        /// <param name="app">A class that provides the mechanisms to configure an application's request.</param>
        /// <param name="corsPolicyName">Optional: Name of the CORS policy to re-add to the specific details back to the response header.</param>
        /// <param name="includeStackTrace">Wether a stacktrace is included in the output or not.</param>
        /// <param name="customExceptionFunction">Optional: Custom function to handle any unique logic when an exception is handled. For example, what do you do if a custom exception is thrown? You can handle any custom code, here.</param>
        /// <returns>This class that provides the mechanisms to configure an application's request</returns>
        public static IApplicationBuilder UseAllResponsesAsJson(this IApplicationBuilder app,
                                                                string corsPolicyName = null,
                                                                bool includeStackTrace = false,
                                                                Func<Exception, JsonExceptionPageResult> customExceptionFunction = null)
        {
            var statusCodePagesOptions = new StatusCodePagesOptions
            {
                HandleAsync = (context) =>
                {
                    // NOTE: Based upon Microsoft source code for their StatusCode middleware:
                    // - https://github.com/aspnet/Diagnostics/blob/4e044a1e30454b87edbc316f40ba608d1160cb28/src/Microsoft.AspNetCore.Diagnostics/StatusCodePage/StatusCodePagesExtensions.cs#L84
                    // - https://github.com/aspnet/Diagnostics/blob/4e044a1e30454b87edbc316f40ba608d1160cb28/src/Microsoft.AspNetCore.Diagnostics/StatusCodePage/StatusCodePagesOptions.cs#L20
                    var body = staticCodeDictionary.GetOrAdd(context.HttpContext.Response.StatusCode,
                                                             statusCode => BuildResponseBody(statusCode));
                    //var body = BuildResponseBody(context.HttpContext.Response.StatusCode);
                    context.HttpContext.Response.ContentType = JsonContentType;
                    return context.HttpContext.Response.WriteAsync(body);
                }
            };

            return app.UseStatusCodePages(statusCodePagesOptions)
                      .UseJsonExceptionPage(corsPolicyName,
                                            includeStackTrace,
                                            customExceptionFunction);
        }

        /// <summary>
        /// Adds a simple json error message for a WebApi/REST website.
        /// </summary>
        /// <param name="app">A class that provides the mechanisms to configure an application's request.</param>
        /// <param name="corsPolicyName">Optional: Name of the CORS policy to re-add to the specific details back to the response header.</param>
        /// <param name="includeStackTrace">Wether a stacktrace is included in the output or not.</param>
        /// <param name="customExceptionFunction">Optional: Custom function to handle any unique logic when an exception is handled. For example, what do you do if a custom exception is thrown? You can handle any custom code, here.</param>
        /// <returns>This class that provides the mechanisms to configure an application's request</returns>
        public static IApplicationBuilder UseJsonExceptionPage(this IApplicationBuilder app,
                                                               string corsPolicyName = null,
                                                               bool includeStackTrace = false,
                                                               Func<Exception, JsonExceptionPageResult> customExceptionFunction = null)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            
            return app.UseExceptionHandler(options => options.Run(
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

            JsonExceptionPageResult jsonExceptionPageResult = null;
            if (customExceptionFunction != null)
            {
                jsonExceptionPageResult = customExceptionFunction(exception);
            }
            
            // It's possible that result contains no result/nothing passed
            // -or-
            // no custom exception logic was provided..
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
        
        private static string BuildResponseBody(int statusCode)
        {
            var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);
            var message = $"Status Code: {statusCode}{(string.IsNullOrWhiteSpace(reasonPhrase) ? "" : "; ")}{reasonPhrase}";
            var error = new ApiErrorResult(message);
            return JsonConvert.SerializeObject(error, JsonHelpers.JsonSerializerSettings);
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
