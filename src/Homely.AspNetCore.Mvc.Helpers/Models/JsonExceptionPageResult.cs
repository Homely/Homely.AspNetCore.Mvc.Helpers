using System.Collections.Generic;
using System.Net;

namespace Homely.AspNetCore.Mvc.Helpers.Models
{
    public class JsonExceptionPageResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public IEnumerable<ApiError> ApiErrors { get; set; }
    }
}
