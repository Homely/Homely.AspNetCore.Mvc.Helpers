using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace Homely.AspNetCore.Mvc.Helpers.Filters
{
    // REF: https://andrewlock.net/using-cancellationtokens-in-asp-net-core-mvc-controllers/

    /// <summary>
    /// Filter to capture when an <code>OperationCanceledException</code> exception has occurred.
    /// </summary>
    public class OperationCancelledExceptionFilter : ExceptionFilterAttribute
    {
        private const int ClientClosedRequestStatusCode = 499;
        private readonly ILogger _logger;
        private readonly int _statusCode;
        private readonly string _httpStatusesUri;

        public OperationCancelledExceptionFilter(int statusCode = ClientClosedRequestStatusCode,
                                                 ILoggerFactory loggerFactory = null)
        {
            if (statusCode <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(statusCode), "'StatusCode' should be a valid status code. Need help determining a status code? Try referencing a wiki on Http Status Codes: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes");
            }

            _statusCode = statusCode;
            _httpStatusesUri = $"https://httpstatuses.com/{statusCode}";
            _logger = loggerFactory?.CreateLogger<OperationCancelledExceptionFilter>();
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is OperationCanceledException)
            {
                _logger.LogDebug("Request was cancelled by User/Source computer.");

                var error = new ProblemDetails
                {
                    Type = _httpStatusesUri,
                    Title = "Request was cancelled.",
                    Status = _statusCode,
                    Instance = context.HttpContext.Request.Path
                };

                context.ExceptionHandled = true;
                context.Result =   new ObjectResult(error)
                {
                    StatusCode = _statusCode
                };
            }
        }
    }
}