using Newtonsoft.Json;
using System.Collections.Generic;

namespace Homely.AspNetCore.Mvc.Helpers.Models
{
    /// <summary>
    /// Error model for an API.
    /// </summary>
    public class ApiErrorResult
    {
        public ApiErrorResult(string message,
                              string stackTrace = null) : this(new ApiError(message), stackTrace)
        {
        }

        public ApiErrorResult(ApiError error,
                              string stackTrace = null) : this(new ApiError[] { error }, stackTrace)
        {
        }

        [JsonConstructor]
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