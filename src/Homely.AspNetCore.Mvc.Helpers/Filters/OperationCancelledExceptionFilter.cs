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
        private readonly ILogger _logger;
        private readonly int _statusCode;

        public OperationCancelledExceptionFilter(ILoggerFactory loggerFactory, int statusCode = 499)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (statusCode <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(statusCode), "'StatusCode' should be a valid status code. Need help determining a status code? Try referencing a wiki on Http Status Codes: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes");
            }

            _logger = loggerFactory.CreateLogger<OperationCancelledExceptionFilter>();
            _statusCode = statusCode;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is OperationCanceledException)
            {
                _logger.LogDebug("Request was cancelled by User/Source computer.");
                
                context.ExceptionHandled = true;
                context.Result = new StatusCodeResult(_statusCode);
            }
        }
    }
}