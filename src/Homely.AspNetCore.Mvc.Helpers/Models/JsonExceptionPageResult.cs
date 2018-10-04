using System.Collections.Generic;
using System.Net;

namespace Homely.AspNetCore.Mvc.Helpers.Models
{
    /// <summary>
    /// JSON Exception page data.
    /// </summary>
    public class JsonExceptionPageResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public IEnumerable<ApiError> ApiErrors { get; set; }
    }
}
