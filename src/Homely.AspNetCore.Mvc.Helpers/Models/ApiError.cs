using System;

namespace Homely.AspNetCore.Mvc.Helpers.Models
{
    /// <summary>
    /// Standardized api error message.
    /// </summary>
    public class ApiError
    {
        
        public ApiError()
        {   
        }
        
        public ApiError(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException(nameof(errorMessage));
            }
            
            Message = errorMessage;
        }

        /// <summary>
        /// Optional: Key to help represent the error message.
        /// </summary>
        /// <remarks>This key might be used to help connect/link any error message to some source, like some html/css for client side error messages, etc.</remarks>
        public string Key { get; set; }

        /// <summary>
        /// The error message.
        /// </summary>
        public string Message { get; set; }
    }
}
