using System.Collections.Generic;

namespace Homely.AspNetCore.Mvc.Helpers.Models
{
    public class ApiErrorResult
    {
        public ApiErrorResult(IEnumerable<ApiError> errors,
                              string stackTrace)
        {
            Errors = errors;
            StackTrace = stackTrace;
        }

        public IEnumerable<ApiError> Errors { get; }
        public string StackTrace { get; }
    }
}
