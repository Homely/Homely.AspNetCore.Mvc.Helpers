using Homely.AspNetCore.Mvc.Helpers.Models;
using System.Collections.Generic;

namespace Homely.AspNetCore.Mvc.Helpers.ViewModels
{
    /// <summary>
    /// A simple error view model to help represent our error message(s).
    /// </summary>
    public class ErrorViewModel
    {
        public ErrorViewModel(IEnumerable<ApiError> errors )
        {
            Errors = errors;
        }

        /// <summary>
        /// A collection of errors.
        /// </summary>
        public IEnumerable<ApiError> Errors { get; }

        /// <summary>
        /// OPTIONAL: A custom status code to help further identifier the _type_ of error this is.
        /// </summary>
        public int? CustomStatusCode { get; set; }
    }
}
