using System;
using System.Collections.Generic;
using System.Linq;
using Homely.AspNetCore.Mvc.Helpers.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Homely.AspNetCore.Mvc.Helpers.Extensions
{
    public static class ApiErrorExtensions
    {
        /// <summary>
        /// Converts a Dictionary of key/value error messages, to a nice collection of ApiErrors.
        /// </summary>
        /// <param name="errorMessages">The dictionary of error key/value pairs.</param>
        /// <returns>Collection of ApiErrors.</returns>
        public static IEnumerable<ApiError> ToApiErrors(this IDictionary<string, string> errorMessages)
        {
            if (errorMessages == null)
            {
                throw new ArgumentNullException(nameof(errorMessages));
            }

            return errorMessages.Select(error => new ApiError
            {
                Key = error.Key,
                Message = error.Value
            });
        }

        /// <summary>
        /// Converts a ModelState dictionary of errors, to a nice collection of ApiErrors.
        /// </summary>
        /// <param name="modelState">Model state errors.</param>
        /// <returns>Collection of ApiErrors.</returns>
        public static IEnumerable<ApiError> ToApiErrors(this ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            var apiErrors = new List<ApiError>();
            foreach (var error in modelState)
            {
                var result = error.Value.Errors.Select(keyValue => new ApiError
                {
                    Key = error.Key,
                    Message = keyValue.ErrorMessage
                });
                apiErrors.AddRange(result);
            }

            return apiErrors;
        }
    }
}
